using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class App : Application
    {
        public App() => MainPage = new BaseNavigationPage(new PhotoListPage());
    }
}
