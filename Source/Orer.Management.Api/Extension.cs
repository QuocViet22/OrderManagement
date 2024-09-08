using NLog;
using NLog.Extensions.Logging;
using OrderManagement.Common.Setting;
using OrderManagement.Services.Interface;
using OrderManagement.Services.Service;
    
namespace OrerManagement.Api
{
    public static class Extension
    {
        /// <summary>
        /// Register/Inject Service that using in this system
        /// </summary>
        /// <param name="services"></param>
        public static void CoreRegisterBusinessService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        public static void BaseLoggerUsingNLog(this WebApplicationBuilder webApplicationBuilder)
        {
            //  NLog: Setup NLog for Dependency Injection
            var configFile = new FileInfo(ApplicationOptions.LoggerConfig);
            if (configFile.Exists)
            {
                LogManager.Setup().LoadConfigurationFromFile(configFile.FullName);
                LogManager.Configuration.Variables["myDir"] = configFile.DirectoryName;
                webApplicationBuilder.Logging.AddNLog(LogManager.Configuration);
            }
        }
    }
}
