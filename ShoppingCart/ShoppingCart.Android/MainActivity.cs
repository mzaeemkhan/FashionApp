using Platform = Xamarin.Essentials.Platform;

namespace ShoppingCart.Droid
{
    [Activity(Label = "Fashion App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            MobileAds.Initialize(ApplicationContext, "ca-app-pub-6116207491368126~6169677202");

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());


            //Task.Factory.StartNew(() => {

            //    AdvertisingIdClient.Info idInfo = AdvertisingIdClient.GetAdvertisingIdInfo(this);

            //    var id = idInfo.Id;

            //    var android_id = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            //});

            //      MediationTestSuite.Launch(this, "");
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            // Enable scrolling to the page when the keyboard is enabled
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}