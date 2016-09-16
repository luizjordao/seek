using Dapper;
using MySql.Data.MySqlClient;
using Seek.Entities;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Seek.Queries
{
    public class GetSkuById
    {
        public virtual Sku GetResult(string id)
        {
            using (IDbConnection db = new MySqlConnection(ConfigurationManager.ConnectionStrings["Seek"].ConnectionString))
            {
                return db.Query<Sku>(@"
                    Select
	                    Id, 
                        Name, 
                        Price
                    From
	                    Skus
                    Where 
                        Id = @id
                    ",
                    new { id = id }
                )
                .FirstOrDefault();                    
            }
        }
    }
}