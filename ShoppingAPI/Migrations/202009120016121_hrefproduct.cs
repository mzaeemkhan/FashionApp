namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hrefproduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntity", "ProductHref", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntity", "ProductHref");
        }
    }
}
