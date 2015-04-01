namespace Drs.Model.Account
{
    public static class CurrentUserSettings //: ICurrentUserSettings
    {
        static CurrentUserSettings()
        {
            UserInfo = new UserInfoModel();
        }
        public static UserInfoModel UserInfo { get; set; }
    }
}