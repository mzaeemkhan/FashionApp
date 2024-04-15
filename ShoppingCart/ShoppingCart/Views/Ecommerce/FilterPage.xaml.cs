using ShoppingCart.ViewModels.Catalog;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace EssentialUIKit.Views.ECommerce
{
    /// <summary>
    /// The Filter page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPage" /> class.
        /// </summary>
        /// <param name="catalogPageViewModel"></param>
        public FilterPage(CatalogPageViewModel catalogPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = catalogPageViewModel;
        }
    }
}