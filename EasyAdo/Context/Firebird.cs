
namespace EasyAdo
{
    /// <summary>
    /// Contexto usando banco de dados Firebird
    /// </summary>
    public class FbContext : DbContext
    {
        /// <summary>
        /// Construtor padrão usando Firebird como fonte de dados
        /// </summary>
        public FbContext() 
            : base(DbType.Firebird)
        {
        }
    }
}