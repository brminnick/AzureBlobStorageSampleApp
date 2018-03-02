using AzureBlobStorageSampleApp.Mobile.Shared;

using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace AzureBlobStorageSampleApp.UITests
{
    public class AddPhotosPage : BasePage
    {
        readonly Query _saveButton, _cancelButton;

        public AddPhotosPage(IApp app) : base(app, PageTitles.AddPhotoPage)
        {
            _saveButton = x => x.Marked(AutomationIdConstants.AddPhotoPage_SaveButton);
            _cancelButton = x => x.Marked(AutomationIdConstants.CancelButton);
        }

        public void TapSaveButton()
        {
            App.Tap(_saveButton);
            App.Screenshot("Save Button Tapped");
        }

        public void TapCancelButton()
        {
            App.Tap(_cancelButton);
            App.Screenshot("Cancel Button Tapped");
        }
    }
}
