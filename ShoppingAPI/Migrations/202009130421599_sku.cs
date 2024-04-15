namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sku : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntity", "SKU", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntity", "SKU");
        }
    }
}
