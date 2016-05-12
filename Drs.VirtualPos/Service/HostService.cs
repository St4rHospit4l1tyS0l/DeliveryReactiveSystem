using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;
using Drs.VirtualPos.Vm;
using Drs.VirtualPos.Ws.Service;
using Drs.VirtualPos.Ws.Sync;

namespace Drs.VirtualPos.Service
{
    public class HostService : IDisposable
    {
        private readonly LogVm _mainvm;
        private CancellationTokenSource _cancellationHost = new CancellationTokenSource();
        private Task _hostTask;

        public HostService(LogVm mainvm)
        {
            _mainvm = mainvm;
            _hostTask = new TaskFactory().StartNew(InitWsHost, _cancellationHost.Token);
            VirtualLogService.AddItemToLog = AddItemToLog;
        }

        private void InitWsHost()
        {
            while (true)
            {
                try
                {
                    using (var host = new ServiceHost(typeof(VirtualPosConnect), new Uri("http://localhost:5411/VirtualPosConnect")))
                    {
                        host.AddServiceEndpoint(typeof(IVirtualPosConnect), new BasicHttpBinding(), "VirtualPosConnect");
                        var smb = new ServiceMetadataBehavior{ HttpGetEnabled = true };
                        host.Description.Behaviors.Add(smb);
                        var sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                        sdb.IncludeExceptionDetailInFaults = true;
                        host.Open();
                        Thread.Sleep(Timeout.Infinite);
                        _mainvm.InsertItem("INFO: (WH) Never happen");
                    }
                    
                }
                catch (Exception ex)
                {
                    _mainvm.InsertItem("ERROR: (WH) " + ex.Message);
                    break;
                }

                if (_cancellationHost == null || _cancellationHost.IsCancellationRequested)
                    break;
            }
        }

        public void AddItemToLog(string sLog)
        {
            _mainvm.InsertItem(sLog);
        }

        public void Dispose()
        {
            try
            {
                if (_cancellationHost != null)
                {
                    _cancellationHost.Cancel();
                    _cancellationHost.Dispose();
                }

            }
            catch (Exception ex)
            {
                _mainvm.InsertItem("ERROR: (TK) " + ex.Message);
            }
            finally
            {
                _cancellationHost = null;
            }

            try
            {
                if (_hostTask != null)
                {
                    _hostTask.Dispose();
                }

            }
            catch (Exception ex)
            {
                _mainvm.InsertItem("ERROR: (HT) " + ex.Message);
            }
            finally
            {
                _hostTask = null;
            }
        }
    }
}
