using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : ContentPage
    {
        string templink;
        public WebPage(string link)
        {
            InitializeComponent();

            Title = "Product Website";
            browser.Source = link;
     
            templink = link;
            Content = browser;
            browser.Navigated += (sender, args) =>
            {
                busyindicator.IsBusy = false;
            };
            browser.Navigating += (sender, args) =>
            {
             // var web =  (WebView) sender;
              //var source = (UrlWebViewSource) web.Source;
               busyindicator.IsBusy = true;
            };

        }

    }
}