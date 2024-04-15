using System;
using Microsoft.AppCenter.Crashes;
using ShoppingCart.Commands;
using ShoppingCart.DataService;
using ShoppingCart.Views.Home;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ShoppingCart.ViewModels.AboutUs
{
    /// <summary>
    /// ViewModel of AboutUs templates.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class AboutUsViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="AboutUsViewModel" /> class.
        /// </summary>
        public AboutUsViewModel()
        {
            try
            {
                productDescription =
                    "Developed by Muhammad Zaeem Khan.";
                productIcon = "Icon.png";
                productVersion = VersionTracking.CurrentVersion; ;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
            
        }

        #endregion

        #region Fields

        private string productDescription;

        private string productVersion;

        private string productIcon;

        private DelegateCommand backButtonCommand;
        private DelegateCommand logoutCommand;

        #endregion

        #region Properties        


        public bool IsLoginVisible
        {
            get
            {
                if (App.CurrentUserId > 0) return true;
                return false;
            }

        }


        /// <summary>
        /// Gets or sets the description of a product or a company.
        /// </summary>
        /// <value>The product description.</value>
        public string ProductDescription
        {
            get => productDescription;

            set
            {
                productDescription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the product icon.
        /// </summary>
        /// <value>The product icon.</value>
        public string ProductIcon
        {
            get => productIcon;

            set
            {
                productIcon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the product version.
        /// </summary>
        /// <value>The product version.</value>
        public string ProductVersion
        {
            get => productVersion;

            set
            {
                productVersion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public DelegateCommand BackButtonCommand =>
            backButtonCommand ?? (backButtonCommand = new DelegateCommand(BackButtonClicked));

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        public DelegateCommand LogoutCommand =>
            logoutCommand ?? (logoutCommand = new DelegateCommand(Logout));

        private void Logout(object obj)
        {
            try
            {
                App.CurrentUserId = 0;
                App.UserEmailId = "";
                SQLiteDatabase.Shared.DeleteUserDetail();
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }

            
        }

        #endregion

        #region Methods

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

          
        }

        #endregion
    }
}