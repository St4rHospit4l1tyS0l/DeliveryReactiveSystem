﻿using System;
using System.Management;

namespace Drs.Infrastructure.Hinfo
{
    public static class ManagementExt
    {
        public static string GetKey()
        {
            return GetBoardProductId() + "|" + GetProcessorId();
        }

        public static string GetBoardProductId()
        {

            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            var retVal = String.Empty;
            try
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    retVal = wmi.GetPropertyValue("Product").ToString();
                    
                    if(!String.IsNullOrWhiteSpace(retVal))
                        break;
                }
            }
            catch
            { }

            if (String.IsNullOrWhiteSpace(retVal))
                retVal = "BaseBoard: Unknown";
            
            return retVal;
        }

        public static String GetProcessorId()
        {
            var id = String.Empty;
            try
            {

                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {

                    id = mo.Properties["processorID"].Value.ToString();
                    if (!String.IsNullOrWhiteSpace(id))
                        break;
                }
            }
            catch
            {}

            if (String.IsNullOrWhiteSpace(id))
                id = "ProcessorId: Unknown";
            
            return id;
        }
    }
}