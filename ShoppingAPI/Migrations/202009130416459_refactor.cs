namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AddressEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.ReviewImageEntity", "ReviewId", "dbo.ReviewEntity");
            DropForeignKey("dbo.ReviewEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.ReviewEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.UserCardEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.UserCartEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.UserCartEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.MyOrderEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.MyOrderEntity", "UserId", "dbo.UserEntity");
            DropIndex("dbo.AddressEntity", new[] { "UserId" });
            DropIndex("dbo.ReviewEntity", new[] { "UserId" });
            DropIndex("dbo.ReviewEntity", new[] { "ProductId" });
            DropIndex("dbo.ReviewImageEntity", new[] { "ReviewId" });
            DropIndex("dbo.UserCardEntity", new[] { "UserId" });
            DropIndex("dbo.UserCartEntity", new[] { "UserId" });
            DropIndex("dbo.UserCartEntity", new[] { "ProductId" });
            DropIndex("dbo.MyOrderEntity", new[] { "UserId" });
            DropIndex("dbo.MyOrderEntity", new[] { "ProductId" });
            CreateTable(
                "dbo.BrandEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ProductEntity", "Brand_ID", c => c.Int());
            CreateIndex("dbo.ProductEntity", "Brand_ID");
            AddForeignKey("dbo.ProductEntity", "Brand_ID", "dbo.BrandEntity", "ID");
            DropColumn("dbo.ProductEntity", "OverallRating");
            DropColumn("dbo.ProductEntity", "TotalQuantity");
            DropTable("dbo.AddressEntity");
            DropTable("dbo.ReviewEntity");
            DropTable("dbo.ReviewImageEntity");
            DropTable("dbo.UserCardEntity");
            DropTable("dbo.UserCartEntity");
            DropTable("dbo.MyOrderEntity");
            DropTable("dbo.PaymentEntity");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PaymentMode = c.String(),
                        CardTypeIcon = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MyOrderEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ProductId = c.Int(),
                        TotalQuantity = c.Int(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserCartEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ProductId = c.Int(),
                        TotalQuantity = c.Int(nullable: false),
                        AddedDateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserCardEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        CardNumber = c.String(),
                        PaymentMode = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReviewImageEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReviewImage = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ReviewId = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReviewEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ProductId = c.Int(),
                        ProfileImage = c.String(),
                        CustomerName = c.String(),
                        ReviewedDate = c.DateTime(nullable: false),
                        Rating = c.Double(nullable: false),
                        Comment = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AddressEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MobileNo = c.String(),
                        DoorNo = c.String(),
                        Area = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                        AddressType = c.String(),
                        UserId = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ProductEntity", "TotalQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.ProductEntity", "OverallRating", c => c.Double(nullable: false));
            DropForeignKey("dbo.ProductEntity", "Brand_ID", "dbo.BrandEntity");
            DropIndex("dbo.ProductEntity", new[] { "Brand_ID" });
            DropColumn("dbo.ProductEntity", "Brand_ID");
            DropTable("dbo.BrandEntity");
            CreateIndex("dbo.MyOrderEntity", "ProductId");
            CreateIndex("dbo.MyOrderEntity", "UserId");
            CreateIndex("dbo.UserCartEntity", "ProductId");
            CreateIndex("dbo.UserCartEntity", "UserId");
            CreateIndex("dbo.UserCardEntity", "UserId");
            CreateIndex("dbo.ReviewImageEntity", "ReviewId");
            CreateIndex("dbo.ReviewEntity", "ProductId");
            CreateIndex("dbo.ReviewEntity", "UserId");
            CreateIndex("dbo.AddressEntity", "UserId");
            AddForeignKey("dbo.MyOrderEntity", "UserId", "dbo.UserEntity", "ID");
            AddForeignKey("dbo.MyOrderEntity", "ProductId", "dbo.ProductEntity", "ID");
            AddForeignKey("dbo.UserCartEntity", "UserId", "dbo.UserEntity", "ID");
            AddForeignKey("dbo.UserCartEntity", "ProductId", "dbo.ProductEntity", "ID");
            AddForeignKey("dbo.UserCardEntity", "UserId", "dbo.UserEntity", "ID");
            AddForeignKey("dbo.ReviewEntity", "UserId", "dbo.UserEntity", "ID");
            AddForeignKey("dbo.ReviewEntity", "ProductId", "dbo.ProductEntity", "ID");
            AddForeignKey("dbo.ReviewImageEntity", "ReviewId", "dbo.ReviewEntity", "ID");
            AddForeignKey("dbo.AddressEntity", "UserId", "dbo.UserEntity", "ID");
        }
    }
}
