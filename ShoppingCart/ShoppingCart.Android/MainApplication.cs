using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Xamarin.Facebook.Ads;

namespace ShoppingCart.Droid
{
#if DEBUG
    [Application(Debuggable = true, UsesCleartextTraffic = true,NetworkSecurityConfig = "@xml/network_security_config")]
#else
    [Application(Debuggable = false, UsesCleartextTraffic = true,NetworkSecurityConfig = "@xml/network_security_config")]
#endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

//            if (!AudienceNetworkAds.IsInitialized(this))
//            {

//#if DEBUG
//                AdSettings.TurnOnSDKDebugger(this);
//#endif

//                AudienceNetworkAds.BuildInitSettings(this).Initialize();
//            }


            CrossCurrentActivity.Current.Init(this);

           
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
#if DEBUG
              FirebasePushNotificationManager.Initialize(this,true);
#else
            FirebasePushNotificationManager.Initialize(this, false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                //p.Data.TryGetValue("title", out var title);
                //p.Data.TryGetValue("body", out var message);
                //Notification.Builder builder = new Notification.Builder(this, "FirebasePushNotificationChannel");
                //var intent = new Intent(this, typeof(MainActivity));
                //intent.AddFlags(ActivityFlags.ClearTop);
                //PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);
                //builder.SetContentIntent(pendingIntent)
                //    .SetSmallIcon(Resource.Drawable.Icon)
                //    .SetContentTitle((string)title)
                //    .SetContentText((string)message)
                //    .SetPriority(NotificationCompat.PriorityHigh)
                //    .SetAutoCancel(true);
                //NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                //notificationManager.Notify(123, builder.Build());

            };


        }
    }

}