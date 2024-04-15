namespace ShoppingAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailId = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        ForgotPassword = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Summary = c.String(),
                        Description = c.String(),
                        ActualPrice = c.Double(nullable: false),
                        DiscountPercent = c.Double(nullable: false),
                        OverallRating = c.Double(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        SubCategoryId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SubCategoryEntity", t => t.SubCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.SubCategoryId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ReviewImageEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReviewImage = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        ReviewId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ReviewEntity", t => t.ReviewId)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.SubCategoryEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoryEntity", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.CategoryEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RecentEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        UserId = c.Int(),
                        ViewedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId);
            
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.UserWishlistEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        ProductId = c.Int(),
                        IsFavorite = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.BannerEntity",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BannerImage = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
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
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductEntity", t => t.ProductId)
                .ForeignKey("dbo.UserEntity", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyOrderEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.MyOrderEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.UserWishlistEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.UserWishlistEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.UserCartEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.UserCartEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.UserCardEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.ReviewEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.RecentEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.RecentEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.ProductEntity", "UserId", "dbo.UserEntity");
            DropForeignKey("dbo.ProductEntity", "SubCategoryId", "dbo.SubCategoryEntity");
            DropForeignKey("dbo.SubCategoryEntity", "CategoryId", "dbo.CategoryEntity");
            DropForeignKey("dbo.ReviewEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.ReviewImageEntity", "ReviewId", "dbo.ReviewEntity");
            DropForeignKey("dbo.PreviewImageEntity", "ProductId", "dbo.ProductEntity");
            DropForeignKey("dbo.AddressEntity", "UserId", "dbo.UserEntity");
            DropIndex("dbo.MyOrderEntity", new[] { "ProductId" });
            DropIndex("dbo.MyOrderEntity", new[] { "UserId" });
            DropIndex("dbo.UserWishlistEntity", new[] { "ProductId" });
            DropIndex("dbo.UserWishlistEntity", new[] { "UserId" });
            DropIndex("dbo.UserCartEntity", new[] { "ProductId" });
            DropIndex("dbo.UserCartEntity", new[] { "UserId" });
            DropIndex("dbo.UserCardEntity", new[] { "UserId" });
            DropIndex("dbo.RecentEntity", new[] { "UserId" });
            DropIndex("dbo.RecentEntity", new[] { "ProductId" });
            DropIndex("dbo.SubCategoryEntity", new[] { "CategoryId" });
            DropIndex("dbo.ReviewImageEntity", new[] { "ReviewId" });
            DropIndex("dbo.ReviewEntity", new[] { "ProductId" });
            DropIndex("dbo.ReviewEntity", new[] { "UserId" });
            DropIndex("dbo.PreviewImageEntity", new[] { "ProductId" });
            DropIndex("dbo.ProductEntity", new[] { "UserId" });
            DropIndex("dbo.ProductEntity", new[] { "SubCategoryId" });
            DropIndex("dbo.AddressEntity", new[] { "UserId" });
            DropTable("dbo.PaymentEntity");
            DropTable("dbo.MyOrderEntity");
            DropTable("dbo.BannerEntity");
            DropTable("dbo.UserWishlistEntity");
            DropTable("dbo.UserCartEntity");
            DropTable("dbo.UserCardEntity");
            DropTable("dbo.RecentEntity");
            DropTable("dbo.CategoryEntity");
            DropTable("dbo.SubCategoryEntity");
            DropTable("dbo.ReviewImageEntity");
            DropTable("dbo.ReviewEntity");
            DropTable("dbo.PreviewImageEntity");
            DropTable("dbo.ProductEntity");
            DropTable("dbo.UserEntity");
            DropTable("dbo.AddressEntity");
        }
    }
}
