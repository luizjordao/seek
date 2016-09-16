using FluentMigrator;

namespace Seek.Migrations._2016._09
{
    [Migration(MigrationId)]
    public class InitialDatabase : Migration
    {
        public const long MigrationId = 201609160200;

        private const string SkuTableName = "Skus";
        private const string PricingTableName = "Pricing";
        private const string CustomerTableName = "Customers";

        public override void Up()
        {
            CreateTableCustomers()
                .CreateTableSkus()
                .CreateTablePricing();    
        }

        public override void Down()
        {
            DropTableCustomers()
                .DropTableSkus()
                .DropTablePricing();
        }

        private InitialDatabase CreateTableCustomers()
        {
            Create
                .Table(CustomerTableName)
                .WithColumn("id").AsString(100)
                .WithColumn("name").AsString(100);

            return this;
        }

        private InitialDatabase CreateTableSkus()
        {
            Create
                .Table(SkuTableName)
                .WithColumn("id").AsString(100)
                .WithColumn("name").AsString(100)
                .WithColumn("price").AsDecimal(7, 2);

            return this;
        }

        private InitialDatabase CreateTablePricing()
        {
            Create
               .Table(PricingTableName)
               .WithColumn("SkuId").AsString(100)
               .WithColumn("CustomerId").AsString(100)
               .WithColumn("Discriminator").AsString(20)
               .WithColumn("MinimumQuantity").AsByte()
               .WithColumn("Percentage").AsDecimal(5, 5)
               .WithColumn("Range").AsByte();

            return this;
        }
       
        private InitialDatabase DropTableCustomers()
        {
            Delete.Table(CustomerTableName);

            return this;
        }

        private InitialDatabase DropTableSkus()
        {
            Delete.Table(SkuTableName);

            return this;
        }

        private InitialDatabase DropTablePricing()
        {
            Delete.Table(PricingTableName);

            return this;
        }      
    }

}
