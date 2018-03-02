using Xamarin.UITest;

using AzureBlobStorageSampleApp.Mobile.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace AzureBlobStorageSampleApp.UITests
{
    public class PhotoListPage : BasePage
    {
        readonly Query _photoListView, _addPhotoButton;

        public PhotoListPage(IApp app) : base(app, PageTitles.PhotoListPage)
        {
            _photoListView = x => x.Marked(AutomationIdConstants.PhotoListView);
            _addPhotoButton = x => x.Marked(AutomationIdConstants.AddPhotoButton);
        }

        public void TapAddPhotoButton()
        {
            App.Tap(_addPhotoButton);
            App.Screenshot("Add Photo Button Tapped");
        }

        public void SelectPhoto(string photoTitle)
        {
            App.ScrollDown(photoTitle);
            App.Tap(photoTitle);
            App.Screenshot($"Tapped {photoTitle} From List");
        }
    }
}
