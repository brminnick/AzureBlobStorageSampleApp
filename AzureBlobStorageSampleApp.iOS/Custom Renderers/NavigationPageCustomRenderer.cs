using AzureBlobStorageSampleApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace AzureBlobStorageSampleApp.iOS
{
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var navigationPage = (NavigationPage)Element;
            var navigationPageBackgroundColor = navigationPage.BarBackgroundColor;

            NavigationBar.StandardAppearance.BackgroundColor = navigationPageBackgroundColor == Color.Default
                ? UINavigationBar.Appearance.BarTintColor
                : navigationPageBackgroundColor.ToUIColor();

            NavigationBar.StandardAppearance.TitleTextAttributes = NavigationBar.TitleTextAttributes;
            NavigationBar.StandardAppearance.LargeTitleTextAttributes = NavigationBar.LargeTitleTextAttributes;

            NavigationBar.ScrollEdgeAppearance = NavigationBar.StandardAppearance;
        }
    }
}
