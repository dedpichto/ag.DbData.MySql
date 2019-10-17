using ag.DbData.Abstraction;
using ag.DbData.Abstraction.Services;
using ag.DbData.MySql.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace ag.DbData.MySql.Extensions
{
    /// <summary>
    /// Represents <see cref="ag.DbData.MySql"/> extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Appends the registration of <see cref="MySqlDbDataFactory"/> and <see cref="MySqlDbDataObject"/> services to <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAgMySql(this IServiceCollection services)
        {
            services.TryAddTransient<IDbDataStringProvider, DbDataStringProvider>();
            services.AddSingleton<IMySqlDbDataFactory, MySqlDbDataFactory>();
            services.AddTransient<MySqlDbDataObject>();
            return services;
        }

        /// <summary>
        /// Appends the registration of <see cref="MySqlDbDataFactory"/> and <see cref="MySqlDbDataObject"/> services to <see cref="IServiceCollection"/> and registers a configuration instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> being bound.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAgMySql(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddAgMySql();
            services.Configure<DbDataSettings>(configurationSection);
            return services;
        }

        /// <summary>
        /// Appends the registration of <see cref="MySqlDbDataFactory"/> and <see cref="MySqlDbDataObject"/> services to <see cref="IServiceCollection"/> and configures the options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAgMySql(this IServiceCollection services,
            Action<DbDataSettings> configureOptions)
        {
            services.AddAgMySql();
            services.Configure(configureOptions);
            return services;
        }
    }
}
