using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.System;
using Windows.Foundation;
using Windows.Management.Deployment;
using System.Threading;
using Windows.ApplicationModel.Store.Preview.InstallControl;

namespace WPFLauncherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Uri uri = new Uri("minecraft://DoSomething?With=This");
        const string TargetPackageFamilyName = "Microsoft.MinecraftUWP_8wekyb3d8bbwe";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnActivated(EventArgs e)
        {
            //Test whether the app we want to launch is installed
            var supportStatus = await Launcher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri, TargetPackageFamilyName);
            if (supportStatus != LaunchQuerySupportStatus.Available)
            {
                //Uri uriNew = new Uri("ms-windows-store://pdp/?productid=9NBLGGH2JHXJ");

                AppInstallManager manager = new AppInstallManager();
                var installItems = manager.StartAppInstallAsync("9NBLGGH2JHXJ", "", true, false);
                Status.Text = "Because the app we need is " + supportStatus.ToString();
            }
        }

        private async void LaunchTargetApp_Click(object sender, RoutedEventArgs e)
        {
            var options = new LauncherOptions { TargetApplicationPackageFamilyName = TargetPackageFamilyName };
            bool success = await Launcher.LaunchUriAsync(uri, options);
            Debug.WriteLine(success);
        }
 

 

}
}
