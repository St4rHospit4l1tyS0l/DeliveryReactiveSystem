using System.Collections.Generic;
using Drs.Model.Constants;

namespace Drs.Model.UiView.Shared
{
    public enum StatusScreen
    {
        Login = 1000,
        ShMenu = 2000,
        UmOrd = 3000,
        UmTrc = 4000,
        UmMss = 5000,
        UmStt = 6000,
        UmMsg = SharedConstants.Client.STATUS_SCREEN_MESSAGE
    }

    public static class StatusScreenName
    {
        public const string UM_TRC = "UM_TRC";
        public const string UM_ORD = "UM_ORD";
        public const string AM_STMA = "AM_STMA";
        public const string AM_SE = "AM_SE";
        public const string AM_RE = "AM_RE";
        public const string UM_MSS = "UM_MSS";
        public const string SM_MO = "SM_MO";
        public const string UM_STT = "UM_STT";
        public const string UM_CS = "UM_CS";
        
        public static readonly Dictionary<string, StatusScreen> DicStatusScreen = new Dictionary<string, StatusScreen>
        {
            { UM_ORD, StatusScreen.UmOrd },
            { UM_TRC, StatusScreen.UmTrc },
            { UM_MSS, StatusScreen.UmMss },
            { UM_STT, StatusScreen.UmStt },
            { UM_CS, StatusScreen.Login }
        };

        public static StatusScreen ToEnum(this string value)
        {
            StatusScreen status;
            return DicStatusScreen.TryGetValue(value, out status) ? status : StatusScreen.ShMenu;
        }
    }
}
