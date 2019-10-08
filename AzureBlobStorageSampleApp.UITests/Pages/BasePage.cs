using Xamarin.UITest;

namespace AzureBlobStorageSampleApp.UITests
{
    public abstract class BasePage
    {
        protected BasePage(IApp app, string pageTitle) => (App, Title) = (app, pageTitle);

        public string Title { get; }
        protected IApp App { get; }

        public virtual void WaitForPageToLoad() => App.WaitForElement(Title);
    }
}

