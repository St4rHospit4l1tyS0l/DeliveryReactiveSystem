using System.Collections.Generic;
using System.Linq;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Shared;

namespace Drs.Repository.Client
{
    public class ClientRepository : BaseOneRepository, IClientRepository
    {
        public IEnumerable<ListItemModel> SearchByPhone(string phone, int maxResults)
        {
            return DbEntities.ClientPhone.Where(e => e.Phone.Contains(phone))
                .Select(e => new ListItemModel { Value = e.Phone, Key = e.ClientPhoneId.ToString() })
                .OrderBy(e => e.Value)
                .Take(maxResults)
                .ToList();
        }

        public IEnumerable<ListItemModel> SearchClientsByClientName(string clientName, int maxResults)
        {
            return DbEntities.Client.Where(e => e.FirstName.Contains(clientName) || e.LastName.Contains(clientName))
                .Select(e => new ListItemModel { Value = e.FirstName + " " + e.LastName, Key = e.ClientId.ToString() })
                .OrderBy(e => e.Value)
                .Take(maxResults)
                .ToList();
        }

        public int CountRecurrenceByTime(long dtStart, long dtEnd, int clientId)
        {
            return DbEntities.Recurrence.Count(e => e.ClientId == clientId && e.TimestampShort >= dtStart && e.TimestampShort <= dtEnd);
        }

        public decimal TotalRecurrenceByTotal(long dtStart, long dtEnd, int clientId)
        {
            return DbEntities.Recurrence.Where(e => e.ClientId == clientId && e.TimestampShort >= dtStart && e.TimestampShort <= dtEnd)
                .Sum(e => (decimal?) e.Total) ?? 0;
        }

        public IEnumerable<ListItemModel> SearchByCompany(string company, int maxResults)
        {

            return DbEntities.Company.Where(e => e.Name.Contains(company))
                .Select(e => new ListItemModel { Value = e.Name, Key = e.CompanyId.ToString() })
                .OrderBy(e => e.Value)
                .Take(maxResults)
                .ToList();
        }

        public int GetPhoneIdByPhone(string phone)
        {
            var resPhone = DbEntities.ClientPhone.Where(e => e.Phone.Trim().ToUpper() == phone)
                .Select(e => new { e.ClientPhoneId }).SingleOrDefault();

            return resPhone == null ? SharedConstants.NULL_ID_VALUE : resPhone.ClientPhoneId;
        }

        public IEnumerable<ClientInfoModel> SearchClientsByPhoneId(int phoneId)
        {
            return DbEntities.ClientPhone.Where(e => e.ClientPhoneId == phoneId)
                .SelectMany(e => e.Client.Select(i => new
                {
                    i.ClientId,
                    e.ClientPhoneId,
                    e.Phone,
                    i.BirthDate,
                    i.Company.Name,
                    i.CompanyId,
                    i.Email,
                    i.FirstName,
                    i.LastName,
                    i.ClientPhone
                }))
                .Select(e => new ClientInfoModel
            {
                ClientId = e.ClientId,
                BirthDate = e.BirthDate,
                Company = e.Name,
                CompanyId = e.CompanyId,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                PrimaryPhone = new PhoneModel { Phone = e.Phone, PhoneId = e.ClientPhoneId},
                SecondPhone = e.ClientPhone.Where(i => i.ClientPhoneId != e.ClientPhoneId).Select(i => new PhoneModel{Phone = i.Phone, PhoneId = i.ClientPhoneId}).FirstOrDefault()
            }).ToList();
        }

        public ClientInfoModel GetClientById(int clientId)
        {
            return DbEntities.Client.Where(e => e.ClientId == clientId).Select(e => new ClientInfoModel
            {
                ClientId = e.ClientId,
                BirthDate = e.BirthDate,
                CompanyId = e.CompanyId,
                Company = e.Company.Name,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName
            }).FirstOrDefault();

        }

        public string GetPhoneById(int phoneId)
        {
            return DbEntities.ClientPhone.Where(e => e.ClientPhoneId == phoneId).Select(e => e.Phone).FirstOrDefault();
        }

    }
}   