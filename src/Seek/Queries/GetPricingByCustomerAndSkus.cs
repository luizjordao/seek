using Dapper;
using MySql.Data.MySqlClient;
using Seek.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Seek.Queries
{
    public class GetPricingByCustomerAndSkus
    {
        public virtual IEnumerable<Pricing> GetResult(string customerId, IEnumerable<string> skus)
        {
            using (IDbConnection db = new MySqlConnection(ConfigurationManager.ConnectionStrings["Seek"].ConnectionString))
            {
                return db.Query<dynamic, Sku, Customer, Pricing>(@"
                    Select
                        Discriminator,
                        MinimumQuantity,
                        Percentage,
                        `Range`,
                        SkuId as Id, 
                        Name,
                        Price,
                        CustomerId as Id
                    From 
                        Pricing
                    Inner Join Skus On Skus.Id = Pricing.SkuId
                    Where 
                        CustomerId = @customerId AND SkuId In @skus
                    ", (p, s, c) =>
                    {
                        if (p.Discriminator == "PercentualDiscount")
                        {
                            return new PercentualDiscount(s, c) { MinimumQuantity = (byte)p.MinimumQuantity, Percentage = (decimal)p.Percentage };
                        }
                        else if (p.Discriminator == "NextForFree")
                        {
                            return new NextForFreeDiscount(s, c) { Range = (byte)p.Range };
                        }
                        return p;
                    },
                    new { customerId = customerId, skus = skus},
                    splitOn: "Id, Id");       
                    
            }
        }
    }
}