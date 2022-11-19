using ag.DbData.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;

namespace ag.DbData.MySql.Factories
{
    /// <summary>
    /// Represents MySqlDbDataFactory object.
    /// </summary>
    internal class MySqlDbDataFactory : IMySqlDbDataFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates object of type <see cref="MySqlDbDataObject"/>.
        /// </summary>
        /// <returns><see cref="MySqlDbDataObject"/> implementation of <see cref="IDbDataObject"/> interface.</returns>
        public IDbDataObject Create()
        {
            var dbObject = _serviceProvider.GetService<MySqlDbDataObject>();
            return dbObject;
        }

        /// <summary>
        /// Creates object of type <see cref="MySqlDbDataObject"/>.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns><see cref="MySqlDbDataObject"/> implementation of <see cref="IDbDataObject"/> interface.</returns>
        public IDbDataObject Create(string connectionString)
        {
            var dbObject = _serviceProvider.GetService<MySqlDbDataObject>();
            dbObject.Connection = new MySqlConnection(connectionString);
            return dbObject;
        }

        /// <summary>
        /// Creates object of type <see cref="MySqlDbDataObject"/>.
        /// </summary>
        /// <param name="defaultCommandTimeOut">Replaces default coommand timeout of provider</param>
        /// <returns></returns>
        public IDbDataObject Create(int defaultCommandTimeOut)
        {
            var dbObject = _serviceProvider.GetService<MySqlDbDataObject>();
            dbObject.DefaultCommandTimeout = defaultCommandTimeOut;
            return dbObject;
        }

        /// <summary>
        /// Creates new MySqlDbDataFactory object.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
        public MySqlDbDataFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
