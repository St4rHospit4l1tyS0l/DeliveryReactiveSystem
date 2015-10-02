using System;
using System.Diagnostics;
using System.ServiceModel;
using Drs.Model.Settings;
using Drs.Model.Store;
using Drs.Service.QueryFunction;

namespace Drs.Service.Store
{
    internal class UpdateOrderClient : IDisposable
    {
        private readonly StoreConnection _storeConnection;
        private QueryFunctionClient _client;

        public Guid ConnUid { get; set; }

        public StoreConnection StoreConnection
        {
            get { return _storeConnection; }
        }

        public UpdateOrderClient(StoreConnection storeConnection)
        {
            _storeConnection = storeConnection;
            _client = new QueryFunctionClient(new BasicHttpBinding(), new EndpointAddress(storeConnection.WsAddress + SettingsData.Constants.StoreOrder.WsQueryFunction));
            ConnUid = Guid.NewGuid();
        }

        public ResponseRd CallWsGetOrder(long atoOrderId, EventLog eventLog)
        {
            try
            {
                return _client.GetOrderById(atoOrderId);
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                return null;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) 
                return;
            if (_client == null) 
                return;
            
            try
            {
                _client.Close();
                _client = null;
            }
            catch (Exception)
            {
                _client = null;
            }
        }
    }
}