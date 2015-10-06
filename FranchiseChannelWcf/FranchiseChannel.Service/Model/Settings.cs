using System;
using System.Configuration;

namespace FranchiseChannel.Service.Model
{
    public static class Settings
    {
        public static String DataPath { get; private set; }
        public static String NewDataPath { get; private set; }
        public static String ContainerPath { get; private set; }

        public const String DATA = "DATA";
        public const String NEWDATA = "NEWDATA";

        static Settings()
        {
            DataPath = ConfigurationManager.AppSettings["DataFolder"];
            NewDataPath = ConfigurationManager.AppSettings["NewDataFolder"];
            ContainerPath = ConfigurationManager.AppSettings["ContainerFolder"];
        }
    }
}
