using System;
using System.Data;
using System.Data.Common;
using EasyAdo.Interface;

namespace EasyAdo.Config
{
    class EasyProviderFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _factory;

        public EasyProviderFactory(string providerName)
        {
            _factory = DbProviderFactories.GetFactory(providerName);
        }

        public IDbConnection CreateConnection(string connectionString)
        {
            var connection = _factory.CreateConnection();
            if (connection == null) throw new NullReferenceException("Falha ao criar objeto DbConnection.");

            connection.ConnectionString = connectionString;

            return connection;
        }
    }
}
