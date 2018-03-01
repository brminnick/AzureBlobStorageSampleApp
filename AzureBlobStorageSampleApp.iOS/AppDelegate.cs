using System.Diagnostics;

using UIKit;
using Foundation;

namespace AzureBlobStorageSampleApp.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());
			FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
            EntryCustomReturn.Forms.Plugin.iOS.CustomReturnEntryRenderer.Init();

            EnableAutomationAPIs();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        [Conditional("DEBUG")]
        void EnableAutomationAPIs() => Xamarin.Calabash.Start();
    }
}
