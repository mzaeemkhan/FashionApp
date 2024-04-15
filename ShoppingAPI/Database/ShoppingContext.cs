using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ShoppingAPI.Configuration;
using ShoppingApp.Entities;

namespace ShoppingAPI.Database
{
    /// <summary>
    /// This class is responsible for the value controller.
    /// </summary>
    public class ShoppingContext : DbContext
    {
        /// <summary>
        /// Initializes the shopping context.
        /// </summary>//: base("Data Source=.;Initial Catalog=ShoppingCart;Integrated Security=True")
        /// "Data Source=SQL5074.site4now.net;Initial Catalog=DB_A5E47E_FashionApp;User Id=DB_A5E47E_FashionApp_admin;Password=meeaz2@2;"
        public ShoppingContext() : base("Data Source=.;Initial Catalog=ShoppingCart;Integrated Security=True"
            )
        {
            Configuration.LazyLoadingEnabled = false;
            Database.Initialize(false);
        }

        /// <summary>
        /// On model creating.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);


            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new SubcategoryConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserWishlistConfiguration());
            modelBuilder.Configurations.Add(new RecentConfiguration());
        }

        #region Entity Sets

        /// <summary>
        /// Gets or set the user.
        /// </summary>
        public DbSet<UserEntity> User { get; set; }


        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public DbSet<CategoryEntity> Category { get; set; }

        /// <summary>
        /// Gets or sets the subcategory.
        /// </summary>
        public DbSet<SubCategoryEntity> SubCategory { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        public DbSet<ProductEntity> Product { get; set; }


        /// <summary>
        /// Gets or sets the preview image.
        /// </summary>
      //  public DbSet<PreviewImageEntity> PreviewImage { get; set; }

        /// <summary>
        /// Gets or sets the user wishlist.
        /// </summary>
        public DbSet<UserWishlistEntity> UserWishlist { get; set; }


        /// <summary>
        /// Gets or sets the recent product details.
        /// </summary>
        public DbSet<RecentEntity> Recent { get; set; }

        /// <summary>
        /// Gets or sets the banner.
        /// </summary>
        public DbSet<BannerEntity> Banner { get; set; }

        public DbSet<BrandEntity> Brand { get; set; }

        #endregion
    }
}