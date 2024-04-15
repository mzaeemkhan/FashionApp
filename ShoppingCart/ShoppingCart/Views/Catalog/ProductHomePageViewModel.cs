using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using ShoppingCart.Commands;
using ShoppingCart.DataService;
using ShoppingCart.Models;
using ShoppingCart.Views.Detail;
using ShoppingCart.Views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShoppingCart.ViewModels.Catalog
{
    /// <summary>
    /// ViewModel for home page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ProductHomePageViewModel : BaseViewModel
    {
        #region Constructor

        public ProductHomePageViewModel(IProductHomeDataService productHomeDataService,
            ICatalogDataService catalogDataService)
        {
            try
            {
                BusyTitle = "Opening Dashboard";
                IsBusy = true;
                this.productHomeDataService = productHomeDataService;
                this.catalogDataService = catalogDataService;

                Device.BeginInvokeOnMainThread(async () =>
                {
                   await FetchBannerImage();
                   await FetchOfferProducts();
                   await FetchRecentProducts();
                   await FetchRecommendedProducts();
                });

                //this.itemSelectedCommand = new DelegateCommand(this.ItemSelected);

            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
            finally
            {
                IsBusy = false;
            }
          
        }

        #endregion

        #region Fields

        private ObservableCollection<Product> newArrivalProduts;

        private ObservableCollection<Product> offerProduts;

        private ObservableCollection<Product> recommendedProduts;

        private DelegateCommand itemSelectedCommand;

        private DelegateCommand viewAllCommand;

        private DelegateCommand masterPageOpenCommand;

        private string bannerImage;
        private readonly IProductHomeDataService productHomeDataService;
        private readonly ICatalogDataService catalogDataService;

        private bool isRecentProductExists;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with Xamarin.Forms Image, which displays the banner image.
        /// </summary>
        [DataMember(Name = "bannerimage")]
        public string BannerImage
        {
            get => App.BaseImageUrl + bannerImage;
            set
            {
                bannerImage = value;
                OnPropertyChanged();
            }
        }

        public List<Banner> banners { get; set; }
        [DataMember(Name = "banners")]
        public List<Banner> Banners
        {
            get => banners;
            set
            {
                banners = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>
        public ObservableCollection<Product> NewArrivalProducts
        {
            get => newArrivalProduts;

            set
            {
                if (newArrivalProduts == value) return;

                newArrivalProduts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>
        public ObservableCollection<Product> OfferProducts
        {
            get => offerProduts;

            set
            {
                if (offerProduts == value) return;

                offerProduts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with list view, which displays the collection of products from json.
        /// </summary>
        public ObservableCollection<Product> RecommendedProducts
        {
            get => recommendedProduts;

            set
            {
                if (recommendedProduts == value) return;

                recommendedProduts = value;
                OnPropertyChanged();
            }
        }

        public bool IsRecentProductExists
        {
            get => isRecentProductExists;
            set
            {
                isRecentProductExists = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets the command that will be executed when an item is selected.
        /// </summary>
        public DelegateCommand ItemSelectedCommand =>
            itemSelectedCommand ?? (itemSelectedCommand = new DelegateCommand(ItemSelected));

        public DelegateCommand ViewAllCommand =>
            viewAllCommand ?? (viewAllCommand = new DelegateCommand(ViewAllProducts));

        public DelegateCommand MasterPageOpenCommand =>
            masterPageOpenCommand ?? (masterPageOpenCommand = new DelegateCommand(MasterPageOpened));

        #endregion

        #region Methods

      


        /// <summary>
        /// This method is used to get the banner images
        /// </summary>
        public async Task FetchBannerImage()
        {
            if (!App.CheckInternet())
            {
                return;
            }

            BusyTitle = "Loading Banners";
            IsBusy = true;
        
            try
            {
                var banners = await productHomeDataService.GetBannersAsync();
                if (banners != null && banners.Count > 0) Banners = banners;
         
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }

        }

        

        /// <summary>
        /// This method is used to get the recent products
        /// </summary>
        public async Task FetchRecentProducts()
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return; 
                }

                BusyTitle = "Loading Latest Products";
                IsBusy = true;
                    var recent = await catalogDataService.GetRecentProductsAsync();
                    if (recent != null && recent.Count > 0)
                    {
                        NewArrivalProducts = new ObservableCollection<Product>(recent);
                    }
                

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task FetchRecommendedProducts()
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }
                BusyTitle = "Loading Recommendations";
                IsBusy = true;
                    var recommends = await catalogDataService.GetRecommendedProductsAsync();
                    if (recommends != null && recommends.Count > 0)
                    {
                        IsRecentProductExists = true;
                        RecommendedProducts = new ObservableCollection<Product>(recommends);
                    }
                

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// This method is used to get the offer product
        /// </summary>
        public async Task FetchOfferProducts()
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }
                BusyTitle = "Loading Offers";
                IsBusy = true;
                var offerProducts = await productHomeDataService.GetOfferProductsAsync();
                if (offerProducts != null && offerProducts.Count > 0)
                {
                    OfferProducts = new ObservableCollection<Product>(offerProducts);
                   
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async void ItemSelected(object attachedObject)
        {
            if (attachedObject != null && attachedObject is Product product && product != null)
                await Application.Current.MainPage.Navigation.PushAsync(new DetailPage(product));
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private async void MasterPageOpened(object attachedObject)
        {
            try
            {
                if (Application.Current.MainPage is NavigationPage &&
                    (Application.Current.MainPage as NavigationPage).CurrentPage is HomePage)
                    ((Application.Current.MainPage as NavigationPage).CurrentPage as MasterDetailPage).IsPresented = true;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
          
        }

        /// <summary>
        /// Invoked when an view all is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void ViewAllProducts(object obj)
        {
            //Do something
        }

        #endregion
    }
}