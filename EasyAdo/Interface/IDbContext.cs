using System;
using System.Data;
using EasyAdo.Builder;

namespace EasyAdo.Interface
{
    /// <summary>
    /// Contrato de configuração do contexto
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Define ou retorna o uso de sequência de conexão para todas as instâncias dessa classe.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Retorna o objeto atual Transaction ou nulo se nenhuma transação estiver em vigor.
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Inicializa objeto de execução
        /// </summary>
        /// <param name="query">Sql de consulta ou nome da stored procedure</param>
        /// <param name="type">Tipo de comando</param>
        /// <param name="parameters">Objeto contendo parâmetros DbParameter</param>
        /// <returns>Objeto contendo métodos de persistência/leitura</returns>
        Executor Execute(string query, CommandType type = CommandType.Text, DbParameterBuilder parameters = null);
        
        /// <summary>
        /// Inicia uma transação
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Envia qualquer transação em vigor.
        /// </summary>
        void Commit();

        /// <summary>
        /// Reverte qualquer transação em vigor.
        /// </summary>
        void Rollback();
    }
}
