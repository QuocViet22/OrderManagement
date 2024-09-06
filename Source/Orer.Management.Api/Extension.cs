using NLog;
using NLog.Extensions.Logging;
using Order.Management.Common.Setting;
using Order.Management.Services.Interface;
using Order.Management.Services.Service;

namespace Orer.Management.Api
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
