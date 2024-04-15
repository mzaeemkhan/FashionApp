namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductEntity", "Type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductEntity", "Type");
        }
    }
}
