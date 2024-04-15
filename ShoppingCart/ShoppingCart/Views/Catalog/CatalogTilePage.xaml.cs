using ShoppingCart.DataService;
using ShoppingCart.ViewModels.Catalog;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views.Catalog
{
    /// <summary>
    /// Page to show Catalog tile.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogTilePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogTilePage" /> class.
        /// </summary>
        public CatalogTilePage(string selectedCategory)
        {
            InitializeComponent();


            var catalogDataService = DataService.TypeLocator.Resolve<ICatalogDataService>();
            var wishlistDataService = App.MockDataService
                ? TypeLocator.Resolve<IWishlistDataService>()
                : DataService.TypeLocator.Resolve<IWishlistDataService>();
            BindingContext = new CatalogPageViewModel(catalogDataService, wishlistDataService,
                selectedCategory);

        
        }

    }
}