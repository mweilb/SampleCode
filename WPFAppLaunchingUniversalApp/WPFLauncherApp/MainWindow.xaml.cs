using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Windows.System;
using Windows.Foundation;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace WPFLauncherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Uri uri = new Uri("minecraft://Mode?Oculus=true");
        const string TargetPackageFamilyName = "Microsoft.MinecraftUWP_8wekyb3d8bbwe";
        bool bLoading = false;
        public MainWindow()
        {
            InitializeComponent();
        }
 
        protected override async void OnActivated(EventArgs e)
        {
            if (bLoading == false)
            {
                
                //Test whether the app we want to launch is installed
                var supportStatus = await Launcher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri, TargetPackageFamilyName);
                if (supportStatus != LaunchQuerySupportStatus.Available)
                {
                    bLoading = true;
                    Status.Text = "We need to install :) " + supportStatus.ToString();

                    AppInstallManager manager = new AppInstallManager();
                    var installItems = await manager.StartAppInstallAsync("9NBLGGH2JHXJ", "0010", true, false);


                    installItems.StatusChanged += new TypedEventHandler<AppInstallItem, System.Object>((app, obj) => StatusChangedUpdate(Status, app, obj));
                    installItems.Completed += new TypedEventHandler<AppInstallItem, System.Object>((app, obj) => CompletedUpdate(Status, app, obj));
                }
                else {
                    LaunchTargetApp.IsEnabled = true;
                }
            }
        }

        private async void LaunchTargetApp_Click(object sender, RoutedEventArgs e)
        {
            var options = new LauncherOptions { TargetApplicationPackageFamilyName = TargetPackageFamilyName };
            bool success = await Launcher.LaunchUriAsync(uri, options);
            Close();
        }

        private void StatusChangedUpdate(TextBlock statusText, AppInstallItem app, System.Object status)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                var statusResult = app.GetCurrentStatus();
                statusText.Text = "We are installing " + statusResult.BytesDownloaded + " of "
                                                       + statusResult.DownloadSizeInBytes;
            }));
    
        }

        private void CompletedUpdate(TextBlock statusText, AppInstallItem app, System.Object status)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                statusText.Text = "Download is good.";
                LaunchTargetApp.IsEnabled = true;
            }));
        }
    }
}
