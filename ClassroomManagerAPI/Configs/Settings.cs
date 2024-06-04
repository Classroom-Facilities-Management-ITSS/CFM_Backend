namespace ClassroomManagerAPI.Configs
{
    public static class Settings
    {
        public const string DefaultConnection = "DefaultConnection";
        public const string AuthenConnection = "AuthenConnection";
        public const string APIVersion = "1.0";
        public const string APIDefaultRoute = "api/v{version:apiVersion}";
        public const string SettingFileName = "appsettings.json";
        public const string ResourcesVerify = "Resources/verify.html";
        public const string ResourecesChangePassword = "Resources/change_password.html";
        public const string ResourecesChangeClass = "Resources/change_class.html";
        public const string ImageFile = "image/jpeg";
        public const string ResourecesNotification = "Resources/notification.html";
        public const string NotificationBody = "< p style = \"margin: 20\" >{0} tại phòng {1}vào lúc {2} đến {3}</p>";
        public const string StorageClass = "Storage";
        public static class Excels
        {
            public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            public const string SheetName = "Sheet1";

            public const string TableName = "Table1";
        }
    }
}
