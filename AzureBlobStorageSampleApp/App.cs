using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace AzureBlobStorageSampleApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var navigationPage = new BaseNavigationPage(new PhotoListPage());
            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }
}
