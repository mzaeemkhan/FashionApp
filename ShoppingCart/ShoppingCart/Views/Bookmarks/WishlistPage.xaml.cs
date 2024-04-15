using ShoppingCart.DataService;
using ShoppingCart.ViewModels.Bookmarks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views.Bookmarks
{
    /// <summary>
    /// Page to show the wishlist.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WishlistPage : ContentPage
    {
        public WishlistPage()
        {
            InitializeComponent();
            var wishlistDataService = TypeLocator.Resolve<IWishlistDataService>();
            BindingContext = new WishlistViewModel(wishlistDataService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is WishlistViewModel wishListVM)
                Device.BeginInvokeOnMainThread(() => { wishListVM.FetchWishlist(); });
        }
    }
}