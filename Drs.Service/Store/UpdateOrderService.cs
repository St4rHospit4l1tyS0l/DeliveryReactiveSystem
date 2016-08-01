using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Drs.Infrastructure.Extensions;
using Drs.Model.Settings;
using Drs.Model.Store;
using Drs.Repository.Store;
using Drs.Service.QueryFunction;

namespace Drs.Service.Store
{
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly EventLog _eventLog;
        private ConnectionPoolWs<UpdateOrderClient> _connectionPool;
        public UpdateOrderService(EventLog eventLog)
        {
            _eventLog = eventLog;
            InitializeConnectionPool();
        }

        private void InitializeConnectionPool()
        {
            _connectionPool = new ConnectionPoolWs<UpdateOrderClient>
            {
                LstConnections = new List<UpdateOrderClient>()
            };

            foreach (var storeConnection in ContainerStoresConnection.Stores)
            {
                _connectionPool.LstConnections.Add(new UpdateOrderClient(storeConnection));
            }
        }

        public void DoUpdateOrderTask(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    GetOrderStatus(token);
                }
                catch (Exception ex)
                {
                    _eventLog.WriteEntry(ex.Message + " -ST- " + ex.StackTrace, EventLogEntryType.Error);
                }

                Task.Delay(TimeSpan.FromSeconds(SettingsData.Store.TimeUpdateStoreOrder), token).Wait(token);
            }
        }

        private void GetOrderStatus(CancellationToken token)
        {
            var tasks = new List<Task>();

            using (var repository = new StoreRepository())
            {
                repository.Db.Configuration.ValidateOnSaveEnabled = false;
                var query = repository.GetCommentsQry();
                var suscribe = query.ToObservable().Subscribe(order =>
                {
                    var conn = _connectionPool.LstConnections.FirstOrDefault(n => n.StoreConnection.StoreId == order.StoreId);
                    if (conn == null)
                        return;

                    tasks.Add(ExecuteGetOrderStatus(order, conn, token));
                });

                Task.WaitAll(tasks.ToArray(), token);
                suscribe.Dispose();
            }
        }

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
        }
    }
}