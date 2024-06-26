using ShoppingCart.ViewModels.Catalog;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace EssentialUIKit.Views.ECommerce
{
    /// <summary>
    /// The Sort page
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortPage" /> class.
        /// </summary>
        /// <param name="catalogPageViewModel"></param>
        public SortPage(CatalogPageViewModel catalogPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = catalogPageViewModel;
        }
    }
}