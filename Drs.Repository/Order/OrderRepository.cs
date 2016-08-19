using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Drs.Model.Address;
using Drs.Model.Client;
using Drs.Model.Order;
using Drs.Repository.Account;
using Drs.Repository.Entities;
using Drs.Repository.Shared;

namespace Drs.Repository.Order
{
    public class OrderRepository : BaseOneRepository, IOrderRepository
    {
        public OrderRepository()
        {
            
        }

        public OrderRepository(CallCenterEntities dbEntities)
            :base(dbEntities)
        {
            
        }

        public PhoneModel GetPhoneByPhone(string phone)
        {
            return DbEntities.ClientPhone.Where(e => e.Phone.Trim().ToUpper() == phone)
                .Select(e => new PhoneModel
                {
                    Phone = e.Phone,
                    PhoneId = e.ClientPhoneId
                }).FirstOrDefault();
        }

        public PhoneModel SavePhone(PhoneModel model)
        {
            var entity = new ClientPhone
            {
                Phone = model.Phone,
                UserIdIns = AccountRepository.GetIdByUsername(model.Username, DbEntities),
                DatetimeIns = DateTime.Now
            };

            DbEntities.ClientPhone.Add(entity);
            DbEntities.SaveChanges();
            model.PhoneId = entity.ClientPhoneId;
            DbEntities.Entry(entity).State = EntityState.Detached;
            return model;
        }

        public int? FindCompanyByName(string companyName)
        {
            var company = DbEntities.Company.Where(e => e.Name.ToUpper() == companyName).Select(e => new { e.CompanyId }).FirstOrDefault();

            if (company == null)
                return null;

            return company.CompanyId;
        }

        public int SaveCompany(string companyName)
        {
            var company = new Company { Description = companyName.ToUpper(), Name = companyName };
            DbEntities.Company.Add(company);
            DbEntities.SaveChanges();
            return company.CompanyId;
        }

        public int SaveClient(ClientInfoModel model, bool bIsNew)
        {
            var client = bIsNew ? new Entities.Client() : DbEntities.Client.Single(e => e.ClientId == model.ClientId);

            var phoneToAdd = new ClientPhone { ClientPhoneId = model.PrimaryPhone.PhoneId };
            ClientPhone phoneSecToAdd = null;
            DbEntities.ClientPhone.Attach(phoneToAdd);

            if (model.SecondPhone != null)
            {
                phoneSecToAdd = new ClientPhone { ClientPhoneId = model.SecondPhone.PhoneId };
                DbEntities.ClientPhone.Attach(phoneSecToAdd);
            }

            for (var i = client.ClientPhone.Count - 1; i >= 0; i--)
            {
                var phone = client.ClientPhone.ElementAt(i);
                client.ClientPhone.Remove(phone);
            }

            client.ClientPhone.Add(phoneToAdd);
            if (phoneSecToAdd != null)
                client.ClientPhone.Add(phoneSecToAdd);

            client.LoyaltyCode = model.LoyaltyCode;
            client.BirthDate = model.BirthDate;
            client.CompanyId = model.CompanyId;
            client.Email = model.Email;
            client.FirstName = model.FirstName;
            client.LastName = model.LastName;

            if (bIsNew)
            {
                client.DatetimeIns = DateTime.Now;
                DbEntities.Client.Add(client);
            }
            DbEntities.SaveChanges();

            return client.ClientId;
        }

        public bool RelPhoneClientExists(ClientPhoneModel model)
        {
            return
                DbEntities.Client.Any(
                    e => e.ClientId == model.ClientId && e.ClientPhone.Any(i => i.ClientPhoneId == model.ClientPhoneId));
        }

        public void RemoveRelPhoneClient(ClientPhoneModel model)
        {
            var client = new Entities.Client { ClientId = model.ClientId };
            var phone = new ClientPhone { ClientPhoneId = model.ClientPhoneId };
            client.ClientPhone.Add(phone);
            DbEntities.Client.Attach(client);
            client.ClientPhone.Remove(phone);
            DbEntities.SaveChanges();
        }

        public bool RelPhoneAddressExists(AddressPhoneModel model)
        {
            return
                DbEntities.Address.Any(
                    e => e.AddressId == model.AddressId && e.ClientPhone.Any(i => i.ClientPhoneId == model.AddressPhoneId));
        }

        public void RemoveRelPhoneAddress(AddressPhoneModel model)
        {
            var address = new Entities.Address { AddressId = model.AddressId };
            var phone = new ClientPhone { ClientPhoneId = model.AddressPhoneId };
            address.ClientPhone.Add(phone);
            DbEntities.Address.Attach(address);
            address.ClientPhone.Remove(phone);
            DbEntities.SaveChanges();
        }

        public int? SavePosCheck(PosCheck model)
        {
            var entity = new PosOrder
            {
                CheckId = model.CheckId,
                FranchiseCode = model.FranchiseCode,
                GuidId = model.GuidId,
                OrderDatetime = DateTime.Now,
                Subtotal = (Decimal)model.SubTotal,
                Taxes = (Decimal)model.Tax,
                Total = (Decimal)model.Total,
                UserId = AccountRepository.GetIdByUsername(model.Username, DbEntities),
            };

            DbEntities.PosOrder.Add(entity);
            DbEntities.SaveChanges();

            foreach (var itemModel in model.LstItems)
            {
                var item = new PosOrderItem
                {
                    CheckItemId = itemModel.CheckItemId,
                    ItemId = itemModel.ItemId,
                    Name = itemModel.RealName,
                    Price = (Decimal) itemModel.Price,
                    LevelItem = (int) itemModel.Level,
                    ParentId = itemModel.Parent != null ? itemModel.Parent.CheckItemId : (long?) null,
                    PosOrderId = entity.PosOrderId
                };

                DbEntities.PosOrderItem.Add(item);
                DbEntities.SaveChanges();
                itemModel.CheckItemId = item.PosOrderItemId;
            }

            model.Id = entity.PosOrderId;
            DbEntities.Entry(entity).State = EntityState.Detached;
            return model.Id;
        }

        public int GetPhoneIdByPhone(string phone)
        {
            return DbEntities.ClientPhone.Where(e => e.Phone == phone).Select(e => e.ClientPhoneId).FirstOrDefault();
        }

        public OrderInfoModel GetPosOrderById(int posOrderId)
        {
            return DbEntities.OrderToStore.Where(e => e.PosOrderId == posOrderId)
                        .OrderByDescending(e => e.OrderToStoreId)
                        .Select(e => new OrderInfoModel
                        {
                            PosOrderId = e.PosOrderId,
                            Phone = e.ClientPhone.Phone,
                            ClientId = e.ClientId,
                            AddressId = e.AddressId
                        }).FirstOrDefault();
        }


        public IEnumerable<LastOrderInfoModel> GetLastNthPosOrderIdByPhoneId(int clientPhoneId)
        {
            return DbEntities.OrderToStore.Where(e => e.ClientPhoneId == clientPhoneId)
            .OrderByDescending(e => e.OrderToStoreId)
            .Select(e => new LastOrderInfoModel
            {
                PosOrderId = e.PosOrderId,
                ClientName = e.Client.FirstName + " " + e.Client.LastName,
                FranchiseName = e.Franchise.Name,
                StoreName = e.FranchiseStore.Name,
                OrderDate = e.StartDatetime,
                Total = e.PosOrder.Total
            }).Take(5).ToList();
        }

        public PosCheck GetPosCheckByOrderId(int posOrderId)
        {
            return DbEntities.PosOrder.Where(e => e.PosOrderId == posOrderId)
                .Select(e => new PosCheck
                {
                    FranchiseCode = e.FranchiseCode,
                    CheckId = e.CheckId,
                    SubTotal = (double) e.Subtotal,
                    Tax = (double) e.Taxes,
                    Total = (double) e.Total,
                    OrderDateTime = e.OrderDatetime,
                    LstItems = e.PosOrderItem.Select(i => new ItemModel
                    {
                        CheckItemId = i.CheckItemId,
                        ItemId = i.ItemId,
                        Name = i.Name,
                        Price = (double)i.Price,
                        Level = i.LevelItem,
                        ParentId = i.ParentId
                    }).ToList(),
                }).FirstOrDefault();
        }

    }
}
