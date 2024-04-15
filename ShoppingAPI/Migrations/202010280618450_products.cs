namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class products : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProductEntity", name: "UserId", newName: "User_ID");
            RenameIndex(table: "dbo.ProductEntity", name: "IX_UserId", newName: "IX_User_ID");
            DropColumn("dbo.ProductEntity", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductEntity", "Type", c => c.String());
            RenameIndex(table: "dbo.ProductEntity", name: "IX_User_ID", newName: "IX_UserId");
            RenameColumn(table: "dbo.ProductEntity", name: "User_ID", newName: "UserId");
        }
    }
}
