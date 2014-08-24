
namespace EasyAdo
{
    /// <summary>
    /// Contexto usando banco de dados MySql
    /// </summary>
    public class MySqlContext : DbContext
    {
        /// <summary>
        /// Construtor padrão usando MySql como fonte de dados
        /// </summary>
        public MySqlContext()
            : base(DbType.MySql)
        {
        }
    }
}
