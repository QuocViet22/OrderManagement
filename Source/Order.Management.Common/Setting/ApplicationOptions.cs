namespace Order.Management.Common.Setting
{
    public class ApplicationOptions
    {
        protected ApplicationOptions() { }
        public static ApplicationOptions Base() { return new ApplicationOptions(); }
        public static ConnectionStringSetting ConnectionStrings {  get; set; } = new ConnectionStringSetting();
        public static string[] AllowedCors {  get; set; } = Array.Empty<string>();
        public static string LoggerConfig { get; set; } = string.Empty;
    }

    /// <summary>
    /// Connection String Model
    /// </summary>
    public class ConnectionStringSetting
    {
        public string OrderManagementConnection {  get; set; } = string.Empty;
    }
}
