using ShoppingCart.DataService;
using ShoppingCart.ViewModels.Catalog;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
//using XF_FacebookAds;

namespace ShoppingCart.Views.Catalog
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductHomePage : ContentPage
    {
        public ProductHomePage()
        {
            InitializeComponent();
          //  AddFacebookAdsControl();
            var productHomeDataService = App.MockDataService
                ? TypeLocator.Resolve<IProductHomeDataService>()
                : DataService.TypeLocator.Resolve<IProductHomeDataService>();
            var catalogDataService = App.MockDataService
                ? TypeLocator.Resolve<ICatalogDataService>()
                : DataService.TypeLocator.Resolve<ICatalogDataService>();
            BindingContext = new ProductHomePageViewModel(productHomeDataService, catalogDataService);
        }

        //private void AddFacebookAdsControl()
        //{
        //    FacebookAdsControl fbAdsControl = new FacebookAdsControl();

        //    fbAdsControl.HeightRequest = 50;

        //    fbAdsControl.PlacementId = "1245746279121215_1247218368974006"; //We create this on developer facebook website

        //    fbAdsControl.Size = FacebookAdsControl.FacebookAdSize.Banner32050;

        //    fbAdsControl.VerticalOptions = LayoutOptions.EndAndExpand;

        //    StackLayoutAds.Children.Add(fbAdsControl);
        //}
    }
}