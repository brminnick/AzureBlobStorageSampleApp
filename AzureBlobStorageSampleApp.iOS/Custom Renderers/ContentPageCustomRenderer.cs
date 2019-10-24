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

            var thisElement = (ContentPage)Element;

            var leftNavList = new List<UIBarButtonItem>();
            var rightNavList = new List<UIBarButtonItem>();

            if (NavigationController?.TopViewController?.NavigationItem is UINavigationItem navigationItem
                && navigationItem.LeftBarButtonItems.Any())
            {
                for (var i = 0; i < thisElement.ToolbarItems.Count; i++)
                {
                    var reorder = thisElement.ToolbarItems.Count - 1;
                    var itemPriority = thisElement.ToolbarItems[reorder - i].Priority;

                    if (itemPriority is 1)
                    {
                        var leftNavItem = navigationItem.RightBarButtonItems[i];
                        leftNavList.Add(leftNavItem);
                    }
                    else if (itemPriority is 0)
                    {
                        var rightNavItem = navigationItem.RightBarButtonItems[i];
                        rightNavList.Add(rightNavItem);
                    }
                }

                navigationItem.SetLeftBarButtonItems(leftNavList.ToArray(), false);
                navigationItem.SetRightBarButtonItems(rightNavList.ToArray(), false);
            }
        }
    }
}