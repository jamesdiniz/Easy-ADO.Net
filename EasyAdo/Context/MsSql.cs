
namespace EasyAdo
{
    /// <summary>
    /// Contexto usando banco de dados MsSql
    /// </summary>
    public class SqlContext : DbContext
    {
        /// <summary>
        /// Construtor padrão usando MsSql como fonte de dados
        /// </summary>
        public SqlContext()
            : base(DbType.MsSql)
        {
        }
    }
}
