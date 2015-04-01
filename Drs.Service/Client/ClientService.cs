using System;
using System.Collections.Generic;
using Drs.Model.Constants;
using Drs.Model.Order;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Client;

namespace Drs.Service.Client
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ListItemModel> SearchByPhone(string phone)
        {
            using (_repository)
            {
                return _repository.SearchByPhone(phone, SettingsData.Server.MaxResultsOnQuery);
            }
        }
        public IEnumerable<ListItemModel> SearchClientsByClientName(string clientName)
        {
            using (_repository)
            {
                return _repository.SearchClientsByClientName(clientName, SettingsData.Server.MaxResultsOnQuery);
            }
        }

        public IEnumerable<ListItemModel> SearchByCompany(string company)
        {
            using (_repository)
            {
                return _repository.SearchByCompany(company, SettingsData.Server.MaxResultsOnQuery);
            }
        }

        public IEnumerable<ClientInfoModel> SearchClientsByPhone(string phone)
        {
            if(String.IsNullOrWhiteSpace(phone))
                return new List<ClientInfoModel>();

            phone = phone.Trim().ToUpper();
            using (_repository)
            {
                var phoneId = _repository.GetPhoneIdByPhone(phone);

                if(phoneId == SharedConstants.NULL_ID_VALUE)
                    return new List<ClientInfoModel>();


                return _repository.SearchClientsByPhoneId(phoneId);
            }
        }

    }
}
