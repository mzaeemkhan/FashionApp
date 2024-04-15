using ShoppingCart.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : TabbedPage
    {
        public HomeTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Alert", "Are you want to close?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();
                    var closeApplication = DependencyService.Get<ICloseApplication>();
                    if (closeApplication != null) closeApplication.CloseApp();
                }
            });

            return true;
        }
    }
}