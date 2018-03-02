using System.Linq;

using Xamarin.UITest;

using AzureBlobStorageSampleApp.Mobile.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace AzureBlobStorageSampleApp.UITests
{
    public class PhotoDetailPage : BasePage
    {
        readonly Query _photoTitleLabel, _photoImage;

        public PhotoDetailPage(IApp app) : base(app, PageTitles.PhotoDetailsPage)
        {
            _photoTitleLabel = x => x.Marked(AutomationIdConstants.PhotoTitleLabel);
            _photoImage = x => x.Marked(AutomationIdConstants.PhotoImage);
        }

        public string PhotoTitle => App.Query(_photoTitleLabel)?.FirstOrDefault()?.Text;

        public void WaitForImageToAppear()
        {
            App.WaitForElement(_photoImage);
            App.Screenshot("Image Appeared");
        }
    }
}
