using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using MarcTron.Plugin;
using MarcTron.Plugin.CustomEventArgs;
using Microsoft.AppCenter.Crashes;
using ShoppingCart.Commands;
using ShoppingCart.DataService;
using ShoppingCart.Models;
using ShoppingCart.Views.Bookmarks;
using ShoppingCart.Views.Detail;
using ShoppingCart.Views.ErrorAndEmpty;
using ShoppingCart.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShoppingCart.ViewModels.Detail
{
    /// <summary>
    /// ViewModel for detail page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class DetailPageViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="DetailPageViewModel" /> class.
        /// </summary>
        public DetailPageViewModel(ICatalogDataService catalogDataService,
            IWishlistDataService wishlistDataService, Product selectedProduct)
        {
            try
            {
                BusyTitle = "Loading Details";
                IsBusy = true;
                
                this.catalogDataService = catalogDataService;
                this.wishlistDataService = wishlistDataService;
                this.selectedProduct = selectedProduct;

                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6116207491368126/2997610579");
                CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-6116207491368126/2811081771");
                CrossMTAdmob.Current.OnInterstitialClosed += (sender, args) =>
                {
                    CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6116207491368126/2997610579");
                };

                CrossMTAdmob.Current.OnRewarded += Current_OnRewarded;
                CrossMTAdmob.Current.OnRewardedVideoAdClosed += Current_OnRewardedVideoAdClosed;

                Device.BeginInvokeOnMainThread(() =>
                {

                    FetchProduct(selectedProduct.ID.ToString());
                    UpdateCartItemCount();
                });

                AddFavouriteCommand = new DelegateCommand(AddFavouriteClicked);
                NotificationCommand = new DelegateCommand(NotificationClicked);
                AddToCartCommand = new DelegateCommand(AddToCartClicked);
                LoadMoreCommand = new DelegateCommand(LoadMoreClicked);
                ShareCommand = new DelegateCommand(ShareClicked);
                VariantCommand = new DelegateCommand(VariantClicked);
                CardItemCommand = new DelegateCommand(CartClicked);
                BackButtonCommand = new DelegateCommand(BackButtonClicked);
               
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

      

        private async void Current_OnRewardedVideoAdClosed(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                BusyTitle = "Opening Link";
                CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-6116207491368126/2811081771");
                if (Reward)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new WebPage(link));
                }
              
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

        private async void Current_OnRewarded(object sender, MTEventArgs e)
        {
            try
            {
                Reward = true;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
          
            Debug.WriteLine($"OnRewarded: {e.RewardType} - {e.RewardAmount}");
        }


        #region Fields
        public bool Reward { get; set; }
        string link;
        private double productRating;

        private Product productDetail;

        private readonly Product selectedProduct;

        private ObservableCollection<Category> categories;

        private bool isFavourite;

        private bool isReviewVisible;

        private int? cartItemCount;
        private readonly ICatalogDataService catalogDataService;
        private readonly IWishlistDataService wishlistDataService;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with SfRotator and labels, which display the product images and
        /// details.
        /// </summary>
        public Product ProductDetail
        {
            get => productDetail;

            set
            {
                if (productDetail == value) return;

                productDetail = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with StackLayout, which displays the categories using ComboBox.
        /// </summary>
        public ObservableCollection<Category> Categories
        {
            get => categories;

            set
            {
                if (categories == value) return;

                categories = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the Favourite.
        /// </summary>
        public bool IsFavourite
        {
            get => isFavourite;
            set
            {
                isFavourite = value;
                OnPropertyChanged();
            }
        }

        

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the cart items count.
        /// </summary>
        public int? CartItemCount
        {
            get => cartItemCount;
            set
            {
                cartItemCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public DelegateCommand AddFavouriteCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Notification button is clicked.
        /// </summary>
        public DelegateCommand NotificationCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the AddToCart button is clicked.
        /// </summary>
        public DelegateCommand AddToCartCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Show All button is clicked.
        /// </summary>
        public DelegateCommand LoadMoreCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Share button is clicked.
        /// </summary>
        public DelegateCommand ShareCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the size button is clicked.
        /// </summary>
        public DelegateCommand VariantCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when cart button is clicked.
        /// </summary>
        public DelegateCommand CardItemCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public DelegateCommand BackButtonCommand { get; set; }

        #endregion

        #region Methods

        ///// <summary>
        ///// This method is used to add the recent product to the database.
        ///// </summary>
        ///// <param name="productId"></param>
        //public async void AddRecentProduct(string productId)
        //{
        //    try
        //    {
        //        if (App.CurrentUserId > 0)
        //        {
        //            var status = await catalogDataService.AddRecentProduct(App.CurrentUserId, int.Parse(productId));
        //            if (status != null && !status.IsSuccess)
        //                await Application.Current.MainPage.DisplayAlert("Message", status.Message, "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

        /// <summary>
        /// This method is used to update the cart item count.
        /// </summary>
        public async void UpdateCartItemCount()
        {
            //try
            //{
            //    if (App.CurrentUserId > 0)
            //    {
            //        var cartItems = await cartDataService.GetCartItemAsync(App.CurrentUserId);
            //        if (cartItems != null) CartItemCount = cartItems.Count;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            //}
        }

        /// <summary>
        /// This method is used to get the product based on product id.
        /// </summary>
        /// <param name="selectedProduct">Product id</param>
        public async void FetchProduct(string selectedProduct)
        {

            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }

                BusyTitle = "Loading Product";
                IsBusy = true;
                if (CrossMTAdmob.Current.IsInterstitialLoaded())
                {
                    CrossMTAdmob.Current.ShowInterstitial();
                }

                var product = await catalogDataService.GetProductByIdAsync(int.Parse(selectedProduct));
                if (product != null)
                {
                    ProductDetail = GetProductDetail(product);
                    link = ProductDetail.ProductHref;
                    var wishlistProducts = await wishlistDataService.GetUserWishlistAsync(App.CurrentUserId);
                    if (wishlistProducts != null && wishlistProducts.Count > 0)
                        ProductDetail.IsFavourite = wishlistProducts.Any(s => s.ID == ProductDetail.ID);
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
        /// This method is used to update the product reviews and ratings.
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns></returns>
        public Product GetProductDetail(Product product)
        {
            IsBusy = true;
            var selectedPoductDetail = new Product();
            selectedPoductDetail = product;
            IsBusy = false;
            return selectedPoductDetail;
        }

        /// <summary>
        /// Invoked when the Favourite button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void AddFavouriteClicked(object obj)
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }

                BusyTitle = "Adding Favorite";
                IsBusy = true;
                if (App.CurrentUserId > 0)
                {
                    if (obj != null && obj is DetailPageViewModel model && model != null)
                    {
                        model.ProductDetail.IsFavourite = model.ProductDetail.IsFavourite ? false : true;
                        var status = await wishlistDataService.AddOrUpdateUserWishlist(App.CurrentUserId,
                            model.ProductDetail.ID, model.ProductDetail.IsFavourite);
                        if (status == null && !status.IsSuccess)
                            model.ProductDetail.IsFavourite = !model.ProductDetail.IsFavourite;
                        else if (status != null && status.IsSuccess)
                            selectedProduct.IsFavourite = ProductDetail.IsFavourite;
                    }
                }
                else
                {
                    var result = await Application.Current.MainPage.DisplayAlert("Message",
                        "Please login to add your favorite item.", "OK", "CANCEL");
                    if (result) Application.Current.MainPage = new NavigationPage(new SimpleLoginPage());
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
        /// Invoked when the Notification button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void NotificationClicked(object obj)
        {
            // Do something
        }

     
        /// <summary>
        /// Invoked when the Cart button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void AddToCartClicked(object obj)
        {
            try
            {
                BusyTitle = "Loading Advertisement";
                IsBusy = true;
                if (obj != null && obj is DetailPageViewModel detailPageViewModel && detailPageViewModel != null)
                {
                    if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                    {
                        CrossMTAdmob.Current.ShowRewardedVideo();
                    }
                    else
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new WebPage(link));
                        //    await Browser.OpenAsync(link, BrowserLaunchMode.SystemPreferred);
                    }

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
        /// Invoked when Load more button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoadMoreClicked(object obj)
        {
            //Do something
        }

        /// <summary>
        /// Invoked when the Share button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ShareClicked(object obj)
        {
            try
            {
                if (obj != null && obj is DetailPageViewModel model)
                {

                        await Share.RequestAsync(new ShareTextRequest
                    {
                        Text = "Check out this product " + model.ProductDetail.ProductHref + Environment.NewLine + "DOWNLOAD FASHION APP",
                        Title = "Share Link"
                    });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
           
        }

        /// <summary>
        /// Invoked when the variant button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void VariantClicked(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when cart icon button is clicked.
        /// </summary>
        /// <param name="obj"></param>
        private async void CartClicked(object obj)
        {
            //if (CartItemCount > 0)
            //    await Application.Current.MainPage.Navigation.PushAsync(new CartPage());
            //else
            //    await Application.Current.MainPage.Navigation.PushAsync(new EmptyCartPage(false));
        }

        /// <summary>
        /// Invoked when an back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void BackButtonClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}