using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using AutoMapper;
using EssentialUIKit.Views.ECommerce;
using Microsoft.AppCenter.Crashes;
using ShoppingApp.Entities;
using ShoppingCart.Annotations;
using ShoppingCart.Commands;
using ShoppingCart.DataService;
using ShoppingCart.Models;
using ShoppingCart.Views.Bookmarks;
using ShoppingCart.Views.Detail;
using ShoppingCart.Views.ErrorAndEmpty;
using ShoppingCart.Views.Forms;
using ShoppingCart.Views.Home;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShoppingCart.ViewModels.Catalog
{
    /// <summary>
    /// ViewModel for catalog page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CatalogPageViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CatalogPageViewModel" /> class.
        /// </summary>
        public CatalogPageViewModel(ICatalogDataService catalogDataService,
            IWishlistDataService wishlistDataService, string selectedCategory)
        {
            try
            {
                BusyTitle = "Loading Catalog Page";
                IsBusy = true;
                this.catalogDataService = catalogDataService;
                this.wishlistDataService = wishlistDataService;
                SelectedCategory = selectedCategory;


                this.SortOptions = new ObservableCollection<string>
                {
                    "New Arrivals",
                    "Price - high to low",
                    "Price - Low to High",
                    "Discount"
                };

                Device.BeginInvokeOnMainThread(() =>
                {
                    AddBrandsFilters();
                    Products = new ObservableCollection<Product>();
                    AddProducts();
                });
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

       

        public string SelectedCategory { get; set; }

        #endregion

        #region Fields    

        private ObservableCollection<FilterCategory> filterOptions;

        private ObservableCollection<string> sortOptions;

       

        private ObservableCollection<Product> products;

        private DelegateCommand addFavouriteCommand;

        private DelegateCommand itemSelectedCommand;

        public DelegateCommand cardItemCommand;

        private DelegateCommand sortCommand;

        private DelegateCommand filterCommand;

        private DelegateCommand backButtonCommand;

        private int cartItemCount;
        private readonly ICatalogDataService catalogDataService;
        private readonly IWishlistDataService wishlistDataService;

        private bool isEmptyViewVisible;
        private DelegateCommand filterApplyCommand;
        private FilterCategory _filterCategory;
        private DelegateCommand stateChangedCommand;
        private Command loadMoreItemsCommand;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property whether view is visible.
        /// </summary>
        public bool IsEmptyViewVisible
        {
            get => isEmptyViewVisible;

            set
            {
                if (isEmptyViewVisible == value) return;

                isEmptyViewVisible = value;
                OnPropertyChanged();
            }
        }
         
        public FilterCategory filterCategory
        {
            get
            {
                return this._filterCategory;
            }

            set
            {
                if (this._filterCategory == value)
                {
                    return;
                }

                this._filterCategory = value;
                this.OnPropertyChanged();
            }
        }


        public ObservableCollection<FilterCategory> FilterOptions
        {
            get
            {
                return this.filterOptions;
            }

            set
            {
                if (this.filterOptions == value)
                {
                    return;
                }

                this.filterOptions = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the property has been bound with a list view, which displays the sort details.
        /// </summary>
        public ObservableCollection<string> SortOptions
        {
            get
            {
                return this.sortOptions;
            }

            set
            {
                if (this.sortOptions == value)
                {
                    return;
                }

                this.sortOptions = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the item details in tile.
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with view, which displays the cart items count.
        /// </summary>
        public int CartItemCount
        {
            get => cartItemCount;
            set
            {
                cartItemCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Command


        public Command LoadMoreItemsCommand =>
            loadMoreItemsCommand ?? (loadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems));

        private bool CanLoadMoreItems(object obj)
        {
            if (Products.Count >= totalItems)
                return false;
            return true;
        }

        private void LoadMoreItems(object obj)
        {
           
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            BusyTitle = "Loading More Items";
            IsBusy = true;
            listView.IsBusy = true;
           
            var index = Products.Count;
           
            var count = index + 50 >= totalItems ? totalItems - index : 50;
           
            AddProducts(index, count);
           
            listView.IsBusy = false;
        }

        int totalItems;


        private async void AddBrandsFilters()
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }

                BusyTitle = "Loading Brands Filter";
                IsBusy = true;

                var brands = await catalogDataService.GetBrandsAsync();
                if (brands != null && brands.Count > 0)
                {
                    List<BrandHelper> brandHelpers = new List<BrandHelper>();
                    foreach (var brand in brands)
                    {
                        brandHelpers.Add(new BrandHelper
                        {
                            ID = brand.ID,
                            Name = brand.Name,
                            BrandImage = brand.BrandImage,
                            IsBrandChecked = false
                        });
                    }


                    this.FilterOptions = new ObservableCollection<FilterCategory>
                    {

                        new FilterCategory
                        {
                            Name = "Brand",
                            SubFilters = brandHelpers
                           
                        },
                    };
                }

               
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
               // IsBusy = false;
            }



        }

        private async void AddProducts(int index = 0, int count = 50)
        {
            try
            {
                if (!App.CheckInternet())
                {
                    return;
                }

                if (Products.Count == 0)
                {
                    BusyTitle = "Please Wait! Loading First 50 Products";
                }
                else
                {
                    BusyTitle = "Please Wait! Loading Next 50 Products";
                }
                
                IsBusy = true;
                int subCategoryId;
                int.TryParse(SelectedCategory, out subCategoryId);
                var pagedList = await catalogDataService.GetProductBySubCategoryIdAsync(subCategoryId, index, count, sortText, FilterBrandIds);
               var products = Mapper.Map<List<ProductEntity>, List<Product>>(pagedList.products);
               totalItems = pagedList.Totalcounts;
                if (products != null && products.Count > 0)
                {
                  
                       IsEmptyViewVisible = false;
                    foreach (var product in products)
                       {
                        Products.Add(product);
                    }

                    var wishlistProducts = await wishlistDataService.GetUserWishlistAsync(App.CurrentUserId);
                    if (wishlistProducts != null && wishlistProducts.Count > 0)
                        foreach (var product in Products.Where(a => wishlistProducts.Any(s => s.ID == a.ID)))
                            product.IsFavourite = true;

                }
                else
                {
                   // IsEmptyViewVisible = true;
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
        string FilterBrandIds = "";
        public DelegateCommand FilterApplyCommand =>
            filterApplyCommand ?? (filterApplyCommand = new DelegateCommand(FilterApply));

        private async void FilterApply(object obj)
        {
          
           
            
            try
            {
                BusyTitle = "Applying Filter";
                IsBusy = true;
                var BrandIds = FilterOptions[0].SubFilters.Where(x => x.IsBrandChecked == true).Select(x => x.ID).ToList();
                if (BrandIds.Count > 0)
                {
                    FilterBrandIds = string.Join(",", BrandIds);
                }


                IsEmptyViewVisible = false;
                Products = new ObservableCollection<Product>();
                AddProducts(0, 50);


              
                Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
              //  IsBusy = false;
            }
        }



        string sortText = "New Arrivals";
        public DelegateCommand StateChangedCommand =>
            stateChangedCommand ?? (stateChangedCommand = new DelegateCommand(StateChanged));

        private async void StateChanged(object obj)
        {
            BusyTitle = "Sorting Items";
            IsBusy = true;
            if (obj != null)
            {
                if ((obj as SfRadioButton).IsChecked == true)
                {
                 
                    sortText = (obj as SfRadioButton).Text;
                    try
                    {
                        IsBusy = true;
                        Products = new ObservableCollection<Product>();
                        AddProducts(0,50);

                     
                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                    }
                    finally
                    {
                      //  IsBusy = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public DelegateCommand ItemSelectedCommand =>
            itemSelectedCommand ?? (itemSelectedCommand = new DelegateCommand(ItemSelected));

        /// <summary>
        /// Gets or sets the command that will be executed when the Favourite button is clicked.
        /// </summary>
        public DelegateCommand AddFavouriteCommand =>
            addFavouriteCommand ?? (addFavouriteCommand = new DelegateCommand(AddFavouriteClicked));

        /// <summary>
        /// Gets or sets the command that will be executed when the sort button is clicked.
        /// </summary>
        public DelegateCommand SortCommand
        {
            get { return this.sortCommand ?? (this.sortCommand = new DelegateCommand(this.SortClicked)); }
        }

        /// <summary>
        /// Gets or sets the command that will be executed when the filter button is clicked.
        /// </summary>
        public DelegateCommand FilterCommand
        {
            get { return this.filterCommand ?? (this.filterCommand = new DelegateCommand(this.FilterClicked)); }
        }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public DelegateCommand BackButtonCommand =>
            backButtonCommand ?? (backButtonCommand = new DelegateCommand(BackButtonClicked));

        #endregion

        #region Methods

        /// <summary>
        /// This method is used to get the products based on category id
        /// </summary>
        /// <param name="selectedCategory"></param>
        //public async void FetchProducts(string selectedCategory)
        //{
        //    try
        //    {
        //        IsBusy = true;
        //        int subCategoryId;
        //        int.TryParse(selectedCategory, out subCategoryId);
        //        var products = await catalogDataService.GetProductBySubCategoryIdAsync(subCategoryId);
        //        if (products != null && products.Count > 0)
        //        {
        //            IsEmptyViewVisible = false;
        //            Products = new ObservableCollection<Product>(products.OrderByDescending(x => x.UpdatedDate));
        //            tempproducts = new ObservableCollection<Product>(products);
        //            var wishlistProducts = await wishlistDataService.GetUserWishlistAsync(App.CurrentUserId);
        //            if (wishlistProducts != null && wishlistProducts.Count > 0)
        //                foreach (var product in Products.Where(a => wishlistProducts.Any(s => s.ID == a.ID)))
        //                    product.IsFavourite = true;
        //        }
        //        else
        //        {
        //            IsEmptyViewVisible = true;
        //        }


        //        IsBusy = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Crashes.TrackError(ex);
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}


       
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
        /// Invoked when the items are sorted.
        /// </summary>
        /// <param name="attachedObject">The Object</param>
        private void SortClicked(object attachedObject)
        {
            // Do something
            Application.Current.MainPage.Navigation.PushAsync(new SortPage(this));
        }

        /// <summary>
        /// Invoked when the filter button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void FilterClicked(object obj)
        {
            // Do something
            Application.Current.MainPage.Navigation.PushAsync(new FilterPage(this));
        }

        /// <summary>
        /// Invoked when the favourite button is clicked.
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

                BusyTitle = "";
                IsBusy = true;
                if (App.CurrentUserId > 0)
                {
                    if (obj != null && obj is Product product && product != null)
                    {
                        product.IsFavourite = product.IsFavourite ? false : true;
                        var isFavourite = product.IsFavourite;
                        var status =
                            await wishlistDataService.AddOrUpdateUserWishlist(App.CurrentUserId, product.ID,
                                isFavourite);
                        if (status == null || !status.IsSuccess) product.IsFavourite = !isFavourite;
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
        /// Invoked when an back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void BackButtonClicked(object obj)
        {
            try
            {
                if (Application.Current.MainPage is NavigationPage &&
                    (Application.Current.MainPage as NavigationPage).CurrentPage is HomePage)
                {
                    var mainPage =
                        (((Application.Current.MainPage as NavigationPage).CurrentPage as MasterDetailPage)
                            .Detail as NavigationPage).CurrentPage as TabbedPage;
                    mainPage.CurrentPage = mainPage.Children[0];
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PopAsync();
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

        #endregion
    }

    public class FilterCategory
    {
        public string Name { get; set; }
        public List<BrandHelper> SubFilters { get; set; }
    }

    public class BrandHelper : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the banner image.
        /// </summary>
        public string Name { get; set; }
        public string BrandImage { get; set; }


        private bool isBrandChecked = false;
        public bool IsBrandChecked
        {
            get { return isBrandChecked; }
            set
            {
                isBrandChecked = value;
                OnPropertyChanged(nameof(IsBrandChecked));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}