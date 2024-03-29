using Android.App;
using Android.OS;
using AndroidX.Core.App;
using ProgressBarApp.Droid.Services;
using System;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace ProgressBarApp.Droid.Services
{
    public class NotificationService : INotificationService
    {
        private const string CHANNEL_ID = "localNotification";
        private const string CHANNEL_NAME = "channelName";
        private const string CHANNEL_DESCRIPTION = "channelDescription";
        private const int PROGRESS_BAR_MAX = 100;
        private bool isChannelInitialized = false;
        NotificationManager notificationManager;

        public void ShowNotification()
        {
            if (!isChannelInitialized)
            {
                CreateChannel();
            }

            Random random = new Random();
            int notificationId = random.Next();
            int valueProgressBar = 0;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(
                Android.App.Application.Context, CHANNEL_ID)
                .SetContentTitle("Progress Bar")
                .SetContentText($"{valueProgressBar}%")
                .SetSmallIcon(Resource.Drawable.notification)
                .SetProgress(PROGRESS_BAR_MAX, 0, false);

            Thread.Sleep(1000);

            for (int i = 10; i <= 100; i += 10)
            {
                Thread.Sleep(1000);
                valueProgressBar = i;

                builder.SetProgress(PROGRESS_BAR_MAX, valueProgressBar, false)
                    .SetContentText($"{valueProgressBar}%");

                notificationManager.Notify(notificationId, builder.Build());
            }
        }

        // Create the channel (for Android 8.0 and above)
        private void CreateChannel()
        {
            try
            {
                notificationManager = (NotificationManager)Android.App.Application.Context
                    .GetSystemService(Android.Content.Context.NotificationService);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channelNameJava = new Java.Lang.String(CHANNEL_NAME);
                    var channel = new NotificationChannel(CHANNEL_ID, channelNameJava, 
                        NotificationImportance.Default)
                    {
                        Description = CHANNEL_DESCRIPTION
                    };
                    notificationManager.CreateNotificationChannel(channel);
                }

                isChannelInitialized = true;
            }
            catch (Exception ex)
            {
                _ = ex.Message;
                isChannelInitialized = false;
            }
        }
    }
}