using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace EasyAdo.Interface
{
    /// <summary>
    /// Contrato contendo métodos de execução async
    /// </summary>
    public interface IExecutorAsync
    {
        /// <summary>
        /// Executa uma consulta que retorna o número de linhas afetadas.
        /// </summary>
        /// <returns>Retorna o número de linhas afetadas</returns>
        Task<int> AsNonQueryAsync();

        /// <summary>
        /// Executa uma consulta que retorna um único valor.
        /// </summary>
        /// <returns>Retorna o valor da primeira coluna/linha dos resultados</returns>
        Task<object> AsScalarAsync();

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataReader.
        /// </summary>
        /// <returns>Retona um objeto DataReader</returns>
        Task<IDataReader> AsDataReaderAsync();

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataSet.
        /// </summary>
        /// <param name="adapter">Implementação do DataAdapter</param>
        /// <returns>Retorna um objeto DataSet</returns>
        Task<DataSet> AsDataSetAsync(DbDataAdapter adapter);

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataTable.
        /// </summary>
        /// <param name="adapter">Implementação do DataAdapter</param>
        /// <returns>Resultados como um objeto DataTable</returns>
        Task<DataTable> AsDataTableAsync(DbDataAdapter adapter);
    }
}
