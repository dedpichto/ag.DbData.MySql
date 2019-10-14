using ag.DbData.Abstraction.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ag.DbData.MySql.Services
{
    /// <summary>
    /// Represents <see cref="MySqlStringProviderFactory"/> object.
    /// </summary>
    public class MySqlStringProviderFactory : IDbDataStringProviderFactory<MySqlStringProvider>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates new instance of <see cref="MySqlStringProviderFactory"/>.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        public MySqlStringProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates object of type <see cref="MySqlStringProvider"/>.
        /// </summary>
        /// <returns>Object of type <see cref="MySqlStringProvider"/>.</returns>
        public MySqlStringProvider Get()
        {
            return _serviceProvider.GetService<MySqlStringProvider>();
        }
    }
}
