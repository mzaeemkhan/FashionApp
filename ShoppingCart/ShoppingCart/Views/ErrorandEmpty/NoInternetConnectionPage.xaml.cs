﻿using ShoppingCart.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views.ErrorAndEmpty
{
    /// <summary>
    /// Page to show the no internet connection error
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetConnectionPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoInternetConnectionPage" /> class.
        /// </summary>
        public NoInternetConnectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Device.Idiom == TargetIdiom.Phone) ErrorImage.IsVisible = false;
            }
            else
            {
                ErrorImage.IsVisible = true;
            }
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