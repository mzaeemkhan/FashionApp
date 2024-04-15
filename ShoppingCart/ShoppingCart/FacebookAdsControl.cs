//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using Xamarin.Forms;

//namespace XF_FacebookAds
//{
//    public class FacebookAdsControl:ContentView
//    {
//        public static BindableProperty PlacementIdProperty = BindableProperty.Create(nameof(PlacementId), typeof(string),
//            typeof(FacebookAdsControl), null);
//        public string PlacementId
//        {
//            get { return (string)GetValue(PlacementIdProperty); }
//            set { SetValue(PlacementIdProperty, value); }
//        }

//        public static BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(FacebookAdSize),
//            typeof(FacebookAdsControl), FacebookAdSize.BannerHeight50);


//        public FacebookAdSize Size
//        {
//            get { return (FacebookAdSize)GetValue(SizeProperty); }
//            set { SetValue(SizeProperty, value); }
//        }

//        public event EventHandler Loaded;
//        public event EventHandler Click;
//        public event EventHandler<ErrorEventArgs> Error;
//        public event EventHandler Impression;

//        public virtual void OnLoaded()
//        {
//            Loaded?.Invoke(this, EventArgs.Empty);
//            Debug.WriteLine("FACEBOOK ADS LOADED");
//        }

//        public virtual void OnClick()
//        {
//            Click?.Invoke(this, EventArgs.Empty);
//            Debug.WriteLine("FACEBOOK ADS CLICKED");
//        }

//        public virtual void OnImpression()
//        {
//            Impression?.Invoke(this, EventArgs.Empty);
//            Debug.WriteLine("FACEBOOK ADS Impression");
//        }

//        public void OnError(int errorCode, string errorMessage)
//        {
//            Error?.Invoke(this, new ErrorEventArgs(errorCode, errorMessage));
//            Debug.WriteLine("FACEBOOK ADS ERROR: " + errorMessage);
//            //Settings.FbAdsError = errorMessage;
//            //Debug.WriteLine("Settings.FbAdsError: " + Settings.FbAdsError);
//        }

//        public enum FacebookAdSize
//        {
//            Banner32050,
//            BannerHeight50,
//            BannerHeight90,
//            Interstitial,
//            RectangleHeight250
//        }


//        public class ErrorEventArgs : EventArgs
//        {
//            public ErrorEventArgs(int errorCode, string errorMessage)
//            {
//                ErrorCode = errorCode;
//                ErrorMessage = errorMessage;
//            }

//            public int ErrorCode { get; }
//            public string ErrorMessage { get; }

//        }
//    }
//}
