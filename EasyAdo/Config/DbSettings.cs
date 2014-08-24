using System.Configuration;

namespace EasyAdo.Config
{
    /// <summary>
    /// Classe auxiliar para acessar configurações do aplicativo
    /// </summary>
    public class DbSettings
    {
        #region Internal Members

        #endregion

        #region Fields

        /// <summary>
        /// Conexão desta instância
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Nome do provedor desta intância
        /// </summary>
        public string ProviderName { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public DbSettings()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Carrega configurações com base no arquivo de configuração
        /// </summary>
        /// <param name="connectionStringName">Nome da connectionString</param>
        /// <returns>Objeto contendo as informações de conexão</returns>
        public static DbSettings FromConfig(string connectionStringName)
        {
            return FromConfig(GetSettingsFromName(connectionStringName));
        }

        /// <summary>
        /// Carrega configurações com base no arquivo de configuração
        /// </summary>
        /// <param name="connectionString">Conexão para esta instância</param>
        /// <param name="providerName">Nome do provedor para esta intância</param>
        /// <returns>Objeto contendo as informações de conexão</returns>
        public static DbSettings FromConfig(string connectionString, string providerName)
        {
            return new DbSettings
            {
                ConnectionString = connectionString,
                ProviderName = providerName
            };
        }

        /// <summary>
        /// Carrega configurações com base no arquivo de configuração
        /// </summary>
        /// <param name="connectionStringSettings">Objeto contendo configurações de conexão</param>
        /// <returns>Objeto contendo as informações de conexão</returns>
        public static DbSettings FromConfig(ConnectionStringSettings connectionStringSettings)
        {
            return new DbSettings
            {
                ConnectionString = connectionStringSettings.ConnectionString,
                ProviderName = connectionStringSettings.ProviderName
            };
        }

        /// <summary>
        /// Método interno para pegar objeto com configurações usando o nome da string
        /// </summary>
        /// <param name="connectionStringName">Nome da connectionString</param>
        /// <returns>Objeto contendo as informações de conexão</returns>
        private static ConnectionStringSettings GetSettingsFromName(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName];
        }

        #endregion
    }
}