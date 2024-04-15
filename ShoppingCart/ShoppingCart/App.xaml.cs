using MarcTron.Plugin;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using ShoppingCart.DataService;
using ShoppingCart.Mapping;
using ShoppingCart.Views.ErrorAndEmpty;
using ShoppingCart.Views.Home;
using ShoppingCart.Views.Onboarding;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShoppingCart
{
    public partial class App : Application
    {
        public static string BaseUri = "http://192.168.10.10:44352/api/";

        public static bool MockDataService = false;
        public static bool onErrorPage = false;
        public App()
        {
            try
            {
                CrossMTAdmob.Current.TestDevices = new List<string>(new List<string>()
                {
                    "95ECFC851D00B90F8DFDBB735420B95D"
                });
                CrossMTAdmob.Current.UserPersonalizedAds = true;

                //  CrossMTAdmob.Current.AdsId = "ca-app-pub-6116207491368126/5635382208";




                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");

                AppCenter.Start("",
                    typeof(Analytics), typeof(Crashes));

                InitializeComponent();

                if (MockDataService)
                {
                    TypeLocator.Start();
                    MainPage = new NavigationPage(new OnBoardingAnimationPage());
                }
                else
                {
                    ListenNetworkChanges();
                    if (!SQLiteDatabase.Shared.Initialized) SQLiteDatabase.Shared.Init();

                    DataService.TypeLocator.Start();
                    MapperConfig.Config();
                    GetUserInfo();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        public static string BaseImageUrl { get; } =
            "";

        public static int CurrentUserId { get; set; }
        public static string UserEmailId { get; set; }

        public static string UserName { get; set; }

        private void GetUserInfo()
        {
            try
            {
                var userInfo = SQLiteDatabase.Shared.GetUserInfo();
                if (userInfo != null)
                {
                    CurrentUserId = userInfo.UserId;
                    UserEmailId = userInfo.EmailId;
                    UserName = userInfo.UserName;
                    MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    MainPage = new NavigationPage(new OnBoardingAnimationPage());
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private static void ListenNetworkChanges()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private static void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckInternet();
        }


        public static bool CheckInternet()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    if (onErrorPage == false)
                    {
                        onErrorPage = true;
                        Current.MainPage.Navigation.PushAsync(new NoInternetConnectionPage());

                    }
                    return false;
                }
                else if (onErrorPage)
                {
                    Current.MainPage.Navigation.PopAsync();
                    onErrorPage = false;

                }

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return true;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        //Device.BeginInvokeOnMainThread(() =>
                        //{
                        //    mPage.Message = $"{p.Data["body"]}";
                        //});

                    }
                }
                catch (Exception ex)
                {

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                MainPage = new NavigationPage(new HomePage());

            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}