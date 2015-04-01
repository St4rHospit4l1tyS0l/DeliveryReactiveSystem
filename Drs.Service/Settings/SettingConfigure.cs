using System;
using System.Collections.Generic;
using System.Linq;
using Drs.Model.Settings;
using Drs.Model.Shared;
using Drs.Repository.Setting;

namespace Drs.Service.Settings
{
    public static class SettingConfigure
    {
        public static void InitConstants(IDictionary<string, string> dicSettings)
        {
            SettingsData.Server.MaxResultsOnQuery = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.SERVER, SettingsData.Constants.ServerConst.MAX_RESULTS_ON_QUERY, 50);
            
            SettingsData.Client.TotalSecondsToLogOut = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.CLIENT, SettingsData.Constants.ClientConst.TOTAL_SECONDS_TO_LOGOUT, 600);
            SettingsData.Client.MinLengthPhone = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.CLIENT, SettingsData.Constants.ClientConst.MIN_LENGTH_PHONE, 5);
            
            SettingsData.Language = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.SYSTEM, SettingsData.Constants.SystemConst.LANGUAGE, 1);
            SettingsData.FirstRegion = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.SYSTEM, SettingsData.Constants.Control.CONTROL_FIRST_ADDRESS, SettingsData.Constants.Control.CONTROL_COUNTRY);
            SettingsData.LastRegion = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.SYSTEM, SettingsData.Constants.Control.CONTROL_LAST_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_C);
            
            SettingsData.AlohaPath = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.FRANCHISE, SettingsData.Constants.SystemConst.ALOHA_PATH, SettingsData.Constants.Franchise.ALOHA_PATH);
            SettingsData.AlohaIber = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.FRANCHISE, SettingsData.Constants.SystemConst.ALOHA_IBER, SettingsData.Constants.Franchise.ALOHA_IBER);
            SettingsData.AlohaIberToInit = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.FRANCHISE, SettingsData.Constants.SystemConst.ALOHA_IBER_TO_INIT, SettingsData.Constants.Franchise.ALOHA_IBER_TO_INIT);
            SettingsData.SecondsToAskForStatusOrder = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.TRACK, SettingsData.Constants.SystemConst.SECONDS_TO_ASK_FOR_STATUS_ORDER, SettingsData.Constants.TrackConst.SECONDS_TO_ASK_FOR_STATUS_ORDER);
            SettingsData.MinutesToBeFutureOrder = GetIntSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.SystemConst.MINUTES_TO_BE_FUTURE_ORDER, SettingsData.Constants.StoreOrder.MINUTES_TO_BE_FUTURE_ORDER);
            SettingsData.CultureSystem = GetStringSettingValue(dicSettings, SettingsData.Constants.Group.SYSTEM, SettingsData.Constants.SystemConst.CULTURE_SYSTEM, SettingsData.Constants.SystemConst.CULTURE_SYSTEM_DEFAULT);
            

            SettingsData.Store.ByCountry = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_COUNTRY, SettingsData.Constants.SystemConst.FALSE);
            SettingsData.Store.ByRegionA = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_REGIONA, SettingsData.Constants.SystemConst.FALSE);
            SettingsData.Store.ByRegionB = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_REGIONB, SettingsData.Constants.SystemConst.FALSE);
            SettingsData.Store.ByRegionC = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_REGIONC, SettingsData.Constants.SystemConst.FALSE);
            SettingsData.Store.ByRegionD = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_REGIOND, SettingsData.Constants.SystemConst.FALSE);
            SettingsData.Store.ByZipCode = GetBooleanSettingValue(dicSettings, SettingsData.Constants.Group.STORE, SettingsData.Constants.StoreConst.BY_ZIPCODE, SettingsData.Constants.SystemConst.FALSE);
        }

        private static bool GetBooleanSettingValue(IDictionary<string, string> dicSettings, string group, string key, bool bDefault)
        {
            try
            {
                var value = dicSettings[group + SettingsData.Constants.SETTING_SEPARATOR + key];
                switch (value)
                {
                    case "1":
                    case "true":
                    case "TRUE":
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return bDefault;
            }
        }

        public static string GetStringSettingValue(IDictionary<string, string> dicSettings, string group, string key, string sDefault)
        {
            try
            {
                string sValue;
                return dicSettings.TryGetValue(@group + SettingsData.Constants.SETTING_SEPARATOR + key, out sValue) ? sValue : sDefault;
            }
            catch (Exception)
            {
                return sDefault;
            }
        }

        public static int GetIntSettingValue(IDictionary<string, string> dicSettings, string group, string key, int iDefault)
        {
            try
            {
                int iValue;
                if (int.TryParse(dicSettings[group + SettingsData.Constants.SETTING_SEPARATOR + key], out iValue))
                    return iValue;
                return iDefault;
            }
            catch (Exception)
            {
                return iDefault;
            }
        }

        public static void InitControls(IList<ControlTitleModel> lstAddressSetting)
        {
            SettingsData.Constants.AddressGridSetting = new AddressControlSetting
            {
                Country = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_COUNTRY),
                MainAddress = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_MAIN_ADDRESS),
                Reference = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_REFERENCE),
                NumExt = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_NUM_EXT),
                RegionA = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_A),
                RegionB = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_B),
                RegionC = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_C),
                RegionD = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_D),
                ZipCode = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_GRID_ADDRESS, SettingsData.Constants.Control.CONTROL_ZIP_CODE),
            };
            SettingsData.Constants.AddressGridSetting.InitList();

            SettingsData.Constants.AddressUpsertSetting = new AddressControlSetting
            {
                Country = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_COUNTRY),
                MainAddress = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_MAIN_ADDRESS),
                Reference = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_REFERENCE),
                NumExt = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_NUM_EXT),
                RegionA = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_A),
                RegionB = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_B),
                RegionC = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_C),
                RegionD = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_REGION_D),
                ZipCode = GetControlInfo(lstAddressSetting, SettingsData.Constants.Control.CONTAINER_UPSERT_ADDRESS, SettingsData.Constants.Control.CONTROL_ZIP_CODE),
            };

            SettingsData.Constants.AddressUpsertSetting.InitList();

        }

        public static ControlInfoModel GetControlInfo(IEnumerable<ControlTitleModel> lstData, string sContainer, string sControl)
        {
            try
            {
                return lstData.Where(e => e.Container == sContainer && e.ControlName == sControl)
                    .Select(e => new ControlInfoModel
                    {
                        Title = e.Name,
                        IsEnabled = e.IsEnabled,
                        Name = sControl
                    }).Single();
            }
            catch (Exception)
            {
                return new ControlInfoModel();
            }
        }

        public static void Initialize(ISettingRepository repository)
        {
            var dicSettings = repository.FindAll();
            InitConstants(dicSettings);

            var lstAddressSetting = repository.FindAllControlTitlesByLanguage(SettingsData.Language);
            InitControls(lstAddressSetting);
        }
    }
}
