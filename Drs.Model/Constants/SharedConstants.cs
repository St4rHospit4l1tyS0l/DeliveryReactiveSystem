using System.Collections.Generic;
using Drs.Model.Shared;

namespace Drs.Model.Constants
{
    public static class SharedConstants
    {
        public const int LEVEL_PAD_ITEM = 3;
        public const string YES = "YES";
        public const string NO = "NO";
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm";
        public const string DATE_FORMAT = "MM/dd/yyyy";
        
        public const string NOT_APPLICABLE = "N/A";
        public const int NULL_ID_VALUE = 0;
        public const int DEFAULT_INT_VALUE = 0;

        public const string REPORT_DATE_FORMAT = "yyyy/MM/dd";
        public const string REPORT_NO_VALUE = "--";

        public static class Server
        {
            //Module Account
            public const string ACCOUNT_HUB = "ModAccount";
            public const string MENU_INFO_ACCOUNT_HUB_METHOD = "MenuInfo-Account";
            public const string ACCOUNT_INFO_ACCOUNT_HUB_METHOD = "AccountInfo-Account";

            //Module Client
            public const string CLIENT_HUB = "ModClient";
            public const string SEARCH_BY_PHONE_CLIENT_HUB_METHOD = "SearchByPhone-Client";
            public const string SEARCH_BY_COMPANY_CLIENT_HUB_METHOD = "SearchByCompany-Client";
            public const string SEARCH_CLIENTS_BY_PHONE_CLIENT_HUB_METHOD = "SearchClientsByPhone-Client";
            public const string REMOVE_REL_PHONECLIENT_CLIENT_HUB_METHOD = "RemoveRelPhoneClient-Client";
            public const string REMOVE_REL_PHONECLIENT_ADDRESS_HUB_METHOD = "RemoveRelPhoneAddress-Client";
            public const string SEARCH_BY_CLIENTNAME_CLIENT_HUB_METHOD = "SearchByClientName-Client";
            public const string CALCULATE_RECURRENCE_CLIENT_HUB_METHOD = "CalculateRecurrence-Client";

            //Module Order
            public const string ORDER_HUB = "ModOrder";
            public const string LST_FRANCHISE_ORDER_HUB_METHOD = "LstFranchise-Order";
            public const string SAVE_PHONE_ORDER_HUB_METHOD = "SavePhone-Order";
            public const string SAVE_CLIENT_ORDER_HUB_METHOD = "SaveClient-Order";
            public const string SAVE_POS_CHECK_ORDER_HUB_METHOD = "SavePosCheck-Order";
            public const string POS_ORDER_BYID_ORDER_HUB_METHOD = "LastOrder-Order";
            public const string LAST_N_ORDERS_ORDER_HUB_METHOD = "LastNthOrders-Order";
            
            public const string CALCULATE_PRICES_ORDER_HUB_METHOD = "CalculatePrices-Order";

            //Module Address
            public const string ADDRESS_HUB = "ModAddress";
            public const string SEARCH_BY_ZIPCODE_ADDRESS_HUB_METHOD = "SearchByZipCode-Address";
            public const string SEARCH_HIERARCHY_BY_ZIPCODE_ADDRESS_HUB_METHOD = "SearchHierarchyByZipCode-Address";
            public const string FILL_NEXT_LIST_BYNAME_ADDRESS_HUB_METHOD = "FillNextListByName-Address";
            public const string SAVE_ADDRESS_ADDRESS_HUB_METHOD = "SaveAddress-Address";
            public const string SEARCH_ADDRESS_BY_PHONE_ADDRESS_HUB_METHOD = "SearchAddressByPhone-Client";
            
            //Module Pos
            public const string POS_RECEIVER_HUB = "ModPosReceiver";
            public const string ORDER_POS_RECEIVER_HUB_METHOD = "Order-PosReceiver";

            //
            public const string USERNAME_HEADER = "UsrHdr";
            public const string CONNECTION_ID_HEADER = "CONNECTION_ID";

            //Module Store
            public const string STORE_HUB = "ModStore";
            public const string SEND_ORDER_TO_STORE_HUB_METHOD = "SendOrderToStore-Store";
            public const string CANCEL_ORDER_STORE_HUB_METHOD = "CancelOrder-Store";
            public const string AVAILABLE_FOR_ADDRESS_STORE_HUB_METHOD = "AvailableForAddress-Store";
            public const string AVAILABLE_BY_STORE_STORE_HUB_METHOD = "AvailableByStore-Store";
            public const string GET_NOTIFICATIONS_BY_STORE_STORE_HUB_METHOD = "GetNotificationsByStore-Store";

            //Module Track
            public const string TRACK_HUB = "ModTrack";
            public const string SEARCH_BY_PHONE_TRACK_HUB_METHOD = "SearchByPhone-Track";
            public const string SEARCH_BY_CLIENTNAME_TRACK_HUB_METHOD = "SearchByClientName-Track";
            public const string SEARCH_BY_DAILY_INFO_TRACK_HUB_METHOD = "SearchByDailyInfo-Track";
            public const string SHOW_DETAIL_TRACK_HUB_METHOD = "ShowDetail-Track";
            

            //Module Franchise
            public const string FRANCHISE_HUB = "ModFranchise";
            public const string LIST_SYNC_FILES_FRANCHISE_HUB_METHOD = "ListSyncFiles-Franchise";
        }

        public static class Client
        {
            public const string URI_RESOURCE = "Logos\\";

            public const int RECORD_NEW = 1000;
            public const int RECORD_ONPROGRESS_TO_SAVED = 1001;
            public const int RECORD_SAVED = 1002;
            public const int RECORD_ERROR_SAVED = 1003;
            public const int RECORD_ONPROGRESS_TO_PROCESS = 1004;

            public const int STATUS_SCREEN_MESSAGE = 100;
            public const int STATUS_SCREEN_LOGIN = 1000;
            
            public const string IS_TRUE = "TRUE";
            public const string IS_FALSE = "FALSE";

            public const int ORDER_TAB_PHONE = 0;
            public const int ORDER_TAB_FRANCHISE = 1;
            public const int ORDER_TAB_CLIENTS = 2;
            public const int ORDER_TAB_ORDER = 3;
            public const int ORDER_TAB_DELIVERY = 4;

            public const int TRIES_INJECT_POS_DATA = 5;
        
        }
    }
}
