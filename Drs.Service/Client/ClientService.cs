using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Infrastructure.Extensions.Classes;
using Drs.Model.Client.Recurrence;
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

        public ResponseMessageData<RecurrenceResponseModel> CalculateRecurrence(List<int> lstClientId)
        {
            var dtEnd = DateTime.Today;
            var dtEndShort = dtEnd.ToDateShort();

            var recurrenceResponse = new RecurrenceResponseModel();

            using (_repository)
            {
                //Calcular recurrencia en tiempo
                GetRecurrenceByType(lstClientId, dtEnd, recurrenceResponse, SettingsData.Recurrence.LstRecurrenceTypeTime,
                    (dtStart, clientId) => _repository.CountRecurrenceByTime(dtStart, dtEndShort, clientId));

                //Calcular recurrencia en total
                GetRecurrenceByType(lstClientId, dtEnd, recurrenceResponse, SettingsData.Recurrence.LstRecurrenceTypeTotal,
                    (dtStart, clientId) => _repository.TotalRecurrenceByTotal(dtStart, dtEndShort, clientId));
            
            }

            return new ResponseMessageData<RecurrenceResponseModel>
            {
                IsSuccess = true,
                Data = recurrenceResponse
            };
        }

        private void GetRecurrenceByType(List<int> lstClientId, DateTime dtEnd, RecurrenceResponseModel recurrenceResponse, 
            IEnumerable<KeyValuePair<string, RecurrenceType>> lstRecurrenceType, Func<long, int, decimal?> funcGetRecurrenceValue )
        {
            foreach (var typeTime in lstRecurrenceType)
            {
                var dtStart = dtEnd.RecurrenceType(typeTime.Value.Value).ToDateShort();
                foreach (var clientId in lstClientId)
                {
                    var bIsNew = false;
                    var recurrenceClient = recurrenceResponse.LstRecurrence.FirstOrDefault(e => e.ClientId == clientId);

                    if (recurrenceClient == null)
                    {
                        bIsNew = true;
                        recurrenceClient = new RecurrenceClientModel();
                    }

                    var value = funcGetRecurrenceValue(dtStart, clientId);

                    if (value.HasValue == false)
                        value = 0;

                    recurrenceClient.LstName.Add(typeTime.Value.Name);
                    recurrenceClient.LstValue.Add(value.Value);

                    if (!bIsNew)
                        continue;

                    recurrenceClient.ClientId = clientId;
                    recurrenceResponse.LstRecurrence.Add(recurrenceClient);
                }
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
