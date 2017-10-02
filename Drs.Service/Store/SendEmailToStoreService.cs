using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Model.Settings;
using Drs.Model.Store;
using Drs.Repository.Log;
using Drs.Repository.Store;
using Drs.Service.QueryFunction;

namespace Drs.Service.Store
{
    public class SendEmailToStoreService : ISendEmailToStoreService
    {
        private readonly EventLog _eventLog;
        private string _template;

        public SendEmailToStoreService(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void DoSendEmailTask(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    if (_template == null)
                        _template = ReadTemplate(SettingsData.Store.FullPathTemplateEmail);

                    if (_template != null)
                    {
                        var lstEmailsToSend = GetEmailsToSend(SettingsData.Store.MaxTriesSendOrderEmail);

                        SendEmails(lstEmailsToSend, _template);
                    }
                    
    
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }

                Task.Delay(TimeSpan.FromSeconds(SettingsData.Store.TimeSendOrderEmail), token).Wait(token);
            }
        }

        private void SendEmails(List<EmailOrderToStore> lstEmailsToSend, string template)
        {
            foreach (var emailOrder in lstEmailsToSend)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                    SharedLogger.LogError(ex);
                }
            }
        }

        private string ReadTemplate(string fullPathTemplateEmail)
        {
            if (!File.Exists(fullPathTemplateEmail))
                return null;
            return File.ReadAllText(fullPathTemplateEmail);
        }

        private List<EmailOrderToStore> GetEmailsToSend(int maxTriesSendOrderEmail)
        {
            using (var repository = new StoreRepository())
            {
                return repository.GetOrdersToSendByEmail(maxTriesSendOrderEmail);
            }
        }
        /*
        private Task ExecuteGetOrderStatus(TrackOrderModel order, UpdateOrderClient conn, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var response = conn.CallWsGetOrder(long.Parse(order.AtoOrderId), _eventLog);
                ProcessOrderReponse(order, response);
            }, token);
        }

        private void ProcessOrderReponse(TrackOrderModel order, ResponseRd response)
        {
            try
            {
                using (var repository = new StoreRepository())
                {
                    repository.Db.Configuration.ValidateOnSaveEnabled = false;
                    if (response != null && response.IsSuccess && response.Order != null && String.IsNullOrWhiteSpace(response.Order.statusField) == false)
                    {
                        if (order.LastStatus == response.Order.statusField)
                            return;
                        
                        repository.UpdateOrderStatus(order.OrderToStoreId, response.Order.statusField, response.Order.promiseTimeField.ToDateTimeSafe());
                        return;
                    }

                    if(response != null && response.IsSuccess == false)
                        _eventLog.WriteEntry(String.Format("Order Call Failed OrderId: {0} AtoOrderId: {1}  - Error: {2} | {3}", order.OrderToStoreId, order.AtoOrderId,
                            String.IsNullOrWhiteSpace(response.ExcMsg) ? "" : response.ErrMsg, String.IsNullOrWhiteSpace(response.ResultData) ? "" : response.ResultData), EventLogEntryType.Warning);

                    repository.UpdateOrderStatusFailedRetrieve(order.OrderToStoreId, order.FailedStatusCounter);
                }
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
            }
        }*/
    }
}