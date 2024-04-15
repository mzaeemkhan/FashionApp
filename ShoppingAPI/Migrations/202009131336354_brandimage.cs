namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brandimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BrandEntity", "BrandImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BrandEntity", "BrandImage");
        }
    }
}
