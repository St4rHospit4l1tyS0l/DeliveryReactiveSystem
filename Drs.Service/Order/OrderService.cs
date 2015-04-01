using System;
using Drs.Model.Address;
using Drs.Model.Client;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Shared;
using Drs.Repository.Client;

namespace Drs.Service.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }
        public ResponseMessageData<PhoneModel> SavePhone(PhoneModel model, bool bHasToDispose = true)
        {
            if (String.IsNullOrWhiteSpace(model.Phone))
            {
                return new ResponseMessageData<PhoneModel>
                {
                    IsSuccess = false,
                    Message = "No existe un teléfono definido."
                };
            }

            model.Phone = model.Phone.Trim().ToUpper();

            try
            {
                var phone = _repository.GetPhoneByPhone(model.Phone);
                if (phone != null)
                {
                    return new ResponseMessageData<PhoneModel>
                    {
                        Data = phone,
                        IsSuccess = true
                    };
                }

                return new ResponseMessageData<PhoneModel>
                {
                    Data = _repository.SavePhone(model),
                    IsSuccess = true
                };
            }
            finally
            {
                if (bHasToDispose)
                    _repository.Dispose();
            }
        }

        public ResponseMessageData<ClientInfoModel> SaveClient(ClientInfoModel model)
        {
            using (_repository)
            {
                if ((model.CompanyId == null || model.CompanyId == SharedConstants.NULL_ID_VALUE) && !String.IsNullOrWhiteSpace(model.Company))
                {
                    model.CompanyId = SaveCompany(model.Company);
                }

                if (String.IsNullOrWhiteSpace(model.Company))
                    model.CompanyId = null;

                if (model.SecondPhone != null)
                {
                    if (!String.IsNullOrWhiteSpace(model.SecondPhone.Phone) && model.SecondPhone.PhoneId <= SharedConstants.NULL_ID_VALUE)
                    {
                        var secondPhone = SavePhone(model.SecondPhone, false);
                        if (secondPhone.IsSuccess == false)
                            throw new ArgumentNullException(secondPhone.Message);
                        model.SecondPhone.PhoneId = secondPhone.Data.PhoneId;
                    }
                    else
                    {
                        model.SecondPhone = null;
                    }
                }

                model.ClientId = _repository.SaveClient(model, model.ClientId == null || model.ClientId == SharedConstants.NULL_ID_VALUE);

                return new ResponseMessageData<ClientInfoModel>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = String.Empty
                };
            }
        }

        public ResponseMessageData<bool> RemoveRelPhoneClient(ClientPhoneModel model)
        {
            using (_repository)
            {
                if (_repository.RelPhoneClientExists(model))
                {
                    _repository.RemoveRelPhoneClient(model);
                }

                return new ResponseMessageData<bool>
                {
                    Data = true,
                    IsSuccess = true
                };
            }
        }

        public ResponseMessageData<bool> RemoveRelPhoneAddress(AddressPhoneModel model)
        {
            using (_repository)
            {
                if (_repository.RelPhoneAddressExists(model))
                {
                    _repository.RemoveRelPhoneAddress(model);
                }

                return new ResponseMessageData<bool>
                {
                    Data = true,
                    IsSuccess = true
                };
            }    
        }

        public ResponseMessageData<PosCheck> SavePosCheck(PosCheck model)
        {
            using (_repository)
            {
                model.Id = _repository.SavePosCheck(model);

                return new ResponseMessageData<PosCheck>
                {
                    Data = model,
                    IsSuccess = true,
                    Message = String.Empty
                };
            }            
        }

        private int SaveCompany(string companyName)
        {
            companyName = companyName.Trim().ToUpper();
            int? companyId = _repository.FindCompanyByName(companyName);

            if (companyId != null)
                return companyId.Value;

            return _repository.SaveCompany(companyName);
        }
    }
}
