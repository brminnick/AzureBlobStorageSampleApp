using NUnit.Framework;

using Xamarin.UITest;

namespace AzureBlobStorageSampleApp.UITests
{
    [TestFixture(Platform.iOS)]
    [TestFixture(Platform.Android)]
    public abstract class BaseTest
    {
        #region Constructors
        protected BaseTest(Platform platform) => Platform = platform;
        #endregion

        #region Properties
        protected Platform Platform { get; }

        protected AddPhotosPage AddPhotosPage { get; private set; }
        protected PhotoDetailPage PhotoDetailPage { get; private set; }
        protected PhotoListPage PhotoListPage { get; private set; }
        protected IApp App { get; private set; }
        #endregion

        #region Methods
        [SetUp]
        protected virtual void BeforeEachTest()
        {
            App = AppInitializer.StartApp(Platform);

            AddPhotosPage = new AddPhotosPage(App);
            PhotoDetailPage = new PhotoDetailPage(App);
            PhotoListPage = new PhotoListPage(App);

            App.Screenshot("App Launched");
        }

        [TearDown]
        protected virtual void AfterEachTest()
        {

        }

        #endregion
    }
}

