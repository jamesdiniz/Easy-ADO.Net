using EasyAdo.Interface;
using System.Data;
using System.Data.Common;

namespace EasyAdo
{
    /// <summary>
    /// Classe para execução de comandos
    /// </summary>
    public class Executor : IExecutor
    {
        #region Internal Members

        private readonly IDbCommand _cmd;

        #endregion

        #region Ctor

        /// <summary>
        /// Construtor padrão para executar comando
        /// </summary>
        /// <param name="command">Comando associado a conexão</param>
        public Executor(IDbCommand command) 
        {
            _cmd = command;
        }

        #endregion

        #region Exec Members

        /// <summary>
        /// Executa uma consulta que retorna o número de linhas afetadas.
        /// </summary>
        public int AsNonQuery() 
        {
            return _cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executa uma consulta que retorna um único valor
        /// </summary>
        public object AsScalar()
        {
            return _cmd.ExecuteScalar();
        }

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataReader
        /// </summary>
        public IDataReader AsDataReader()
        {
            return _cmd.ExecuteReader();
        }

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataSet
        /// </summary>
        public DataSet AsDataSet(DbDataAdapter adapter)
        {
            var ds = new DataSet();
            adapter.SelectCommand = (DbCommand)_cmd;
            adapter.Fill(ds);
            return ds;
        }

        /// <summary>
        /// Executa uma consulta que retorna os resultados como DataTable
        /// </summary>
        public DataTable AsDataTable(DbDataAdapter adapter)
        {
            var dt = new DataTable();
            adapter.SelectCommand = (DbCommand)_cmd;
            adapter.Fill(dt);
            return dt;
        }

        #endregion
    }
}