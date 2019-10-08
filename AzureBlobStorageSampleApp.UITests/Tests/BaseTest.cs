using NUnit.Framework;

using Xamarin.UITest;

namespace AzureBlobStorageSampleApp.UITests
{
    [TestFixture(Platform.iOS)]
    [TestFixture(Platform.Android)]
    public abstract class BaseTest
    {
        protected BaseTest(Platform platform) => Platform = platform;

        protected Platform Platform { get; }

        protected AddPhotosPage AddPhotosPage { get; private set; }
        protected PhotoDetailPage PhotoDetailPage { get; private set; }
        protected PhotoListPage PhotoListPage { get; private set; }
        protected IApp App { get; private set; }

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
    }
}

