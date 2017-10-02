using System;

namespace Drs.Model.Store
{
    public class EmailOrderToStore
    {
        public string AtoOrderId { get; set; }
        public string StoreName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? PromiseDate { get; set; }
        public EmailClientOrder Client { get; set; }
        public string PhoneNumber { get; set; }
        public EmailAddressOrder Address { get; set; }
        public string ExtraNotes { get; set; }
        public string OrderMode { get; set; }
        public EmailPosOrder PosOrder { get; set; }
        public string Emails { get; set; }
    }

    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
