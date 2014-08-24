using System.Data;
using System.Data.Common;

namespace EasyAdo.Interface
{
    /// <summary>
    /// Contrato contendo métodos de execução
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Executa uma consulta que retorna o número de linhas afetadas.
        /// </summary>
        /// <returns>Retorna o número de linhas afetadas</returns>
        int AsNonQuery();

        /// <summary>
        /// Executa uma consulta que retorna um único valor.
        /// </summary>
        /// <returns>Retorna o valor da primeira coluna/linha dos resultados</returns>
        object AsScalar();

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataReader.
        /// </summary>
        /// <returns>Retona um objeto DataReader</returns>
        IDataReader AsDataReader();

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataSet.
        /// </summary>
        /// <param name="adapter">Implementação do DataAdapter</param>
        /// <returns>Retorna um objeto DataSet</returns>
        DataSet AsDataSet(DbDataAdapter adapter);

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataTable.
        /// </summary>
        /// <param name="adapter">Implementação do DataAdapter</param>
        /// <returns>Resultados como um objeto DataTable</returns>
        DataTable AsDataTable(DbDataAdapter adapter);
    }
}
