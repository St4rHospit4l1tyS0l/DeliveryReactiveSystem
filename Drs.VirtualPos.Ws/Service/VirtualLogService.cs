using System;

namespace Drs.VirtualPos.Ws.Service
{
    public static class VirtualLogService
    {
        public static Action<String> AddItemToLog;

        public static void Write(String sLog)
        {
            var actAddItemToLog = AddItemToLog;
            if(actAddItemToLog == null)
                return;
            actAddItemToLog(sLog);
        }
    }
}
