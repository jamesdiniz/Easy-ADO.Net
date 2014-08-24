using System.Data;

namespace EasyAdo.Interface
{
    /// <summary>
    /// Contrato da fábrica de conexão
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Criar uma conexão do tipo IDbConnection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>Retorna conexão</returns>
        IDbConnection CreateConnection(string connectionString);
    }
}
