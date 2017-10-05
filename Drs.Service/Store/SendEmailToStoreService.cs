using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Model.Settings;
using Drs.Model.Store;
using Drs.Repository.Log;
using Drs.Repository.Store;
using Drs.Service.QueryFunction;
using Newtonsoft.Json;

namespace Drs.Service.Store
{
    public class SendEmailToStoreService : ISendEmailToStoreService
    {
        private readonly EventLog _eventLog;
        private EmailSettings _emailSettings;

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
                    if (_emailSettings == null)
                        _emailSettings = ReadEmailSettings(SettingsData.Store.EmailSettings);

                    if (_emailSettings != null)
                    {
                        var lstEmailsToSend = GetEmailsToSend(SettingsData.Store.MaxTriesSendOrderEmail);

                        if (lstEmailsToSend != null && lstEmailsToSend.Any())
                            SendEmails(lstEmailsToSend, _emailSettings);
                    }
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                    SharedLogger.LogError(ex);
                }

                Task.Delay(TimeSpan.FromSeconds(SettingsData.Store.TimeSendOrderEmail), token).Wait(token);
            }
        }

        private EmailSettings ReadEmailSettings(string emailSettings)
        {
            try
            {
                var settings = JsonConvert.DeserializeObject<EmailSettings>(emailSettings);

                settings.Template = ReadTemplate(settings.TemplatePath);

                if (settings.Template == null)
                    return null;
                
                return settings;
            }
            catch (Exception ex)
            {
                _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                SharedLogger.LogError(ex);
                return null;
            }
        }

        private void SendEmails(List<EmailOrderToStore> lstEmailsToSend, EmailSettings emailSettings)
        {
            using (var repository = new StoreRepository())
            {
                using (var client = new SmtpClient(emailSettings.Host, emailSettings.Port))
                {
                    //client.DeliveryFormat = SmtpDeliveryFormat.International;
                    client.EnableSsl = emailSettings.EnableSsl;
                    client.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                    //client.UseDefaultCredentials = false;

                    foreach (var emailOrder in lstEmailsToSend)
                    {
                        try
                        {
                            emailOrder.TriesToSend++;
                            using (var mail = new MailMessage())
                            {
                                var body = emailOrder.BuildBody(emailSettings.Template);

                                foreach (var destination in emailOrder.DestinationEmails.Split(';'))
                                {
                                    mail.To.Add(new MailAddress(destination));
                                }

                                mail.From = new MailAddress(emailSettings.Sender);
                                mail.Sender = mail.From;
                                mail.Subject = string.Format(emailSettings.Title, emailOrder.AtoOrderId);
                                mail.Body = body;
                                mail.IsBodyHtml = true;
                                client.Send(mail);
                            }

                            repository.UpdateOrderToSendByEmail(emailOrder.OrderToStoreEmailId, emailOrder.TriesToSend, true);
                        }
                        catch (Exception ex)
                        {
                            _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                            SharedLogger.LogError(ex);
                            repository.UpdateOrderToSendByEmail(emailOrder.OrderToStoreEmailId, emailOrder.TriesToSend, false);
                        }
                    }
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
    }
}