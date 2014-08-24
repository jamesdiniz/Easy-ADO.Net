using System.Data.SqlClient;
using EasyAdo.Builder;
using EasyAdo.Config;
using EasyAdo.Interface;
using System;
using System.Data;

namespace EasyAdo
{
    /// <summary>
    /// Classe genérica para trabalhar com bancos de dados usando ADO.net
    /// </summary>
    public class DbContext : IDbContext
    {
        #region Internal Members

        private string _connString;
        private IDbConnection _conn;
        private IDbCommand _command;
        private IDbTransaction _trans;
        private bool _disposed;
        //private DbParameterBuilder _paramBuilder = null;

        #endregion

        #region Fields

        /// <summary>
        /// Retorna a string de conexão atual.
        /// </summary>
        public string ConnectionString
        {
            get { return _connString; }
        }

        /// <summary>
        /// Retorna o objeto atual Transaction ou nulo se nenhuma transação estiver em vigor.
        /// </summary>
        public IDbTransaction Transaction { get { return _trans; } }

        #endregion

        #region Ctor

        /// <summary>
        /// Construtor usando banco de dados padrão.
        /// <para>Esta opção irá buscar no arquivo de configuração uma entrada com nome "DefaultConnectionString".</para>
        /// </summary>
        public DbContext()
        {
            Init(DbSettings.FromConfig("DefaultConnectionString"));
        }

        /// <summary>
        /// Construtor usando banco de dados conforme arquivo de configuração.
        /// </summary>
        /// <param name="connectionStringName">Nome da conexão no arquivo de configuração</param>
        public DbContext(string connectionStringName)
        {
            Init(DbSettings.FromConfig(connectionStringName));
        }

        /// <summary>
        /// Construtor usando string e provedor de conexão
        /// </summary>
        /// <param name="connectionString">String de conexão para esta instância</param>
        /// <param name="providerName">Nome do provedor para esta instância</param>
        public DbContext(string connectionString, string providerName)
        {
            Init(DbSettings.FromConfig(connectionString, providerName));
        }

        /// <summary>
        /// Construtor usando banco de dados conforme o tipo do banco.
        /// <para>Atenção: Requer entrada no arquivo de configuração no formato: [DbType]ConnectionString.</para>
        /// <example>Exemplo: MsSqlConnectionString</example>
        /// </summary>
        /// <param name="type">Tipo de banco de dados</param>
        public DbContext(DbType type)
        {
            Init(DbSettings.FromConfig(type + "ConnectionString"));
        }

        /// <summary>
        /// Construtor usando conexão existente
        /// </summary>
        /// <param name="connection">Conexão existente</param>
        public DbContext(IDbConnection connection)
        {
            Init(connection);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inicializa atributos da classe.
        /// </summary>
        /// <param name="settings"></param>
        protected void Init(DbSettings settings)
        {
            Init(new EasyProviderFactory(settings.ProviderName).CreateConnection(settings.ConnectionString));
        }

        /// <summary>
        /// Inicializa atributos da classe usando conexão existente.
        /// </summary>
        /// <param name="connection">Conexão existente</param>
        protected void Init(IDbConnection connection)
        {
            _connString = connection.ConnectionString;
            _conn = connection;
            _command = _conn.CreateCommand();
            Connect();
        }

        /// <summary>
        /// Abre a conexão com banco de dados. 
        /// </summary>
        private void Connect()
        {
            if (_conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        /// <summary>
        /// Constrói um objeto Command com os parâmetros fornecidos.
        /// </summary>
        /// <param name="query">SQL de consulta ou nome da stored procedure</param>
        /// <param name="type">Tipo de comando SQL</param>
        /// <param name="parameters">Objeto contendo parâmetros DbParameter</param>
        /// <returns></returns>
        private IDbCommand CreateCommand(string query, CommandType type, DbParameterBuilder parameters)
        {
            _command.CommandText = query; // Define a SQL de consulta
            _command.CommandType = type; // Define o tipo de comando

            // Associar com a transação atual, se houver
            if (_trans != null)
                _command.Transaction = _trans;

            // Limpa todos os parametros do comando
            _command.Parameters.Clear();

            // Construe parâmetros
            if (parameters != null)
                foreach (var prs in parameters.Parameters)
                    _command.Parameters.Add(prs);

            return _command;
        }

        #endregion

        #region Exec Members

        /// <summary>
        /// Inicializa objeto de execução
        /// </summary>
        /// <param name="query">Sql de consulta ou nome da stored procedure</param>
        /// <param name="type">Tipo de comando</param>
        /// <param name="parameters">Objeto contendo parâmetros DbParameter</param>
        /// <returns>Objeto contendo métodos de persistência/leitura</returns>
        public Executor Execute(string query, CommandType type = CommandType.Text, DbParameterBuilder parameters = null)
        {
            return new Executor(CreateCommand(query, type, parameters));
        }

        #endregion

        #region Parameter Builder

        /// <summary>
        /// Método para criação de objetos DbParameter
        /// </summary>
        /// <returns></returns>
        public DbParameterBuilder ParameterBuilder()
        {
            // TODO: Colocar parâmetros dentro do contexto e montar comando com base nos valores informados
            //return _paramBuilder ?? (_paramBuilder = new DbParameterBuilder(_command));

            return new DbParameterBuilder(_command);
        }

        //public DbParameterBuilder EasyParameterBuilder
        //{
        //    get { return new DbParameterBuilder(_command); }
        //}

        #endregion

        #region Transaction Members

        /// <summary>
        /// Inicia uma transação
        /// </summary>
        /// <returns>Novo objeto Transaction</returns>
        public void BeginTransaction()
        {
            Rollback();
            _trans = _conn.BeginTransaction();
        }

        /// <summary>
        /// Envia qualquer transação em vigor.
        /// </summary>
        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
                _trans = null;
            }
        }

        /// <summary>
        /// Reverte qualquer transação em vigor.
        /// </summary>
        public void Rollback()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Implementação do IDisposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Limpa recursos utilizados pelo objeto da classe.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_conn != null)
                    {
                        Rollback();
                        _conn.Dispose();
                        _conn = null;
                    }
                }
                _disposed = true;
            }
        }

        #endregion

        #region Enum Helper

        /// <summary>
        /// Tipo de banco de dados suportado
        /// </summary>
        public enum DbType
        {
            /// <summary>
            /// Usa Firebird como fonte de dados
            /// </summary>
            Firebird,

            /// <summary>
            /// Usa MsSql como fonte de dados
            /// </summary>
            MsSql,

            /// <summary>
            /// Usa MySql como fonte de dados
            /// </summary>
            MySql
        }

        #endregion
    }
}