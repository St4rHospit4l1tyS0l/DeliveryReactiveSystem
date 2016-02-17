using Drs.VirtualPos.Ws.Service;

namespace Drs.VirtualPos.Ws.Sync
{
    public class VirtualPosConnect : IVirtualPosConnect
    {
        public string GetData(int value)
        {
            VirtualLogService.Write(value.ToString());
            return string.Format("You entered: {0}", value);
        }
    }
}