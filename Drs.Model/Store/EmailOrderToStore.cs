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
        public string DestinationEmails { get; set; }
        public long OrderToStoreId { get; set; }
        public long OrderToStoreEmailId { get; set; }
        public int TriesToSend { get; set; }

        public string BuildBody(string template)
        {
            return string.Format(template, AtoOrderId, StoreName, Client.GetInfo(), PhoneNumber,
                Address.GetInfo(), GetInfo(), PosOrder.GetInfo());
        }

        private string GetInfo()
        {
            return string.Format("Fecha/hora del pedido: {0}<br/>Fecha/hora de entrega del pedido:{1}Notas:{2}<br/>Modo del pedido:{3}Identificador interno:{4}",
                PosOrder != null ? PosOrder.OrderDate.ToString("yyyy / MM / dd  |  HH:mm:ss") : "ND", 
                PromiseDate.HasValue? PromiseDate.Value.ToString("yyyy / MM / dd  |  HH:mm:ss") : "ND",
                ExtraNotes,
                OrderMode,
                OrderToStoreId);
        }
    }
}
