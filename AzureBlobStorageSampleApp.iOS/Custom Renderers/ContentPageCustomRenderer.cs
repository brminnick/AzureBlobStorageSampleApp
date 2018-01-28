using System.Linq;
using System.Collections.Generic;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using AzureBlobStorageSampleApp.iOS;


[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageCustomRenderer))]
namespace AzureBlobStorageSampleApp.iOS
{
    public class ContentPageCustomRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var thisElement = Element as ContentPage;

            var leftNavList = new List<UIBarButtonItem>();
            var rightNavList = new List<UIBarButtonItem>();

            var navigationItem = NavigationController.TopViewController.NavigationItem;

            if (navigationItem?.LeftBarButtonItems?.Any() == true)
                return;

            for (var i = 0; i < thisElement.ToolbarItems.Count; i++)
            {

                var reorder = (thisElement.ToolbarItems.Count - 1);
                var itemPriority = thisElement.ToolbarItems[reorder - i].Priority;

                if (itemPriority == 1)
                {
                    UIBarButtonItem LeftNavItems = navigationItem.RightBarButtonItems[i];
                    leftNavList.Add(LeftNavItems);
                }
                else if (itemPriority == 0)
                {
                    UIBarButtonItem RightNavItems = navigationItem.RightBarButtonItems[i];
                    rightNavList.Add(RightNavItems);
                }
            }

            navigationItem.SetLeftBarButtonItems(leftNavList.ToArray(), false);
            navigationItem.SetRightBarButtonItems(rightNavList.ToArray(), false);
        }
    }
}