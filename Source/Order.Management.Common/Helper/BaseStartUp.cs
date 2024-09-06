using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Management.Common.Setting;

namespace Order.Management.Common.Helper
{
    public static class BaseStartUp
    {
        /// <summary>
        /// Build base start up for services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection BindingConfiguration(this IServiceCollection services, IConfiguration configuration) 
        {
            services.Configure<ApplicationOptions>(configuration);
            var applicationOptions = ApplicationOptions.Base();
            configuration.Bind(applicationOptions);
            return services;
        }

        public static void AddDBContext<TConText>(this IServiceCollection services, string connectionString) where TConText : DbContext
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            // Add UnitOfWork
            services.AddDbContext<TConText>(option =>
            {
                option.UseSqlServer(connectionString);
#if DEBUG
                option.EnableSensitiveDataLogging(true);
#endif
            }, ServiceLifetime.Scoped).AddUnitOfWork<TConText>();
        }
    }
}
