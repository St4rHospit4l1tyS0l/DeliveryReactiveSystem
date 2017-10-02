using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Drs.Model.Store;

namespace Drs.Service.Email
{
    public class SendEmailService
    {
        public static void SendEmail(EmailSettings emailSettings, string subject, string body, string sender, List<string> destinations)
        {
            using (var mail = new MailMessage())
            {
                foreach (var destination in destinations)
                {
                    mail.To.Add(new MailAddress(destination));
                }

                mail.Sender = new MailAddress(sender);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var client = new SmtpClient(emailSettings.Host, emailSettings.Port))
                {
                    client.DeliveryFormat = SmtpDeliveryFormat.International;
                    client.EnableSsl = emailSettings.EnableSsl;
                    client.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                    client.UseDefaultCredentials = false;
                    client.Send(mail);
                }
            }
        }
    }
}
