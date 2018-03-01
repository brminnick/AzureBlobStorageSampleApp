using System;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace AzureBlobStorageSampleApp.UITests
{
    public abstract class BasePage
    {
        #region Constructors
        protected BasePage(IApp app, string pageTitle)
        {
            App = app;
            Title = pageTitle;
        }
        #endregion

        #region Properties
        public string Title { get; }
        protected IApp App { get; }
        #endregion

        #region Methods
        public virtual void WaitForPageToLoad() => App.WaitForElement(Title);
        #endregion
    }
}

