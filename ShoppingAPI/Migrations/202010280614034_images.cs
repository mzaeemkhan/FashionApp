namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class images : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PreviewImageEntity", "ProductId", "dbo.ProductEntity");
            DropIndex("dbo.PreviewImageEntity", new[] { "ProductId" });
            AddColumn("dbo.ProductEntity", "Image1", c => c.String());
            AddColumn("dbo.ProductEntity", "Image2", c => c.String());
            AddColumn("dbo.ProductEntity", "Image3", c => c.String());
            AddColumn("dbo.ProductEntity", "Image4", c => c.String());
            AddColumn("dbo.ProductEntity", "Image5", c => c.String());
            AddColumn("dbo.ProductEntity", "Image6", c => c.String());
            DropTable("dbo.PreviewImageEntity");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PreviewImageEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        PreviewImage = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.ProductEntity", "Image6");
            DropColumn("dbo.ProductEntity", "Image5");
            DropColumn("dbo.ProductEntity", "Image4");
            DropColumn("dbo.ProductEntity", "Image3");
            DropColumn("dbo.ProductEntity", "Image2");
            DropColumn("dbo.ProductEntity", "Image1");
            CreateIndex("dbo.PreviewImageEntity", "ProductId");
            AddForeignKey("dbo.PreviewImageEntity", "ProductId", "dbo.ProductEntity", "ID", cascadeDelete: true);
        }
    }
}
