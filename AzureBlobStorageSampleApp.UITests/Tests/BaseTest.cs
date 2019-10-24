using System;
using NUnit.Framework;

using Xamarin.UITest;

namespace AzureBlobStorageSampleApp.UITests
{
    [TestFixture(Platform.iOS)]
    [TestFixture(Platform.Android)]
    public abstract class BaseTest
    {
        AddPhotosPage? _addPhotosPage;
        PhotoDetailPage? _photoDetailPage;
        PhotoListPage? _photoListPage;
        IApp? _app;

        protected BaseTest(Platform platform) => Platform = platform;

        protected Platform Platform { get; }

        protected AddPhotosPage AddPhotosPage => _addPhotosPage ?? throw new NullReferenceException();
        protected PhotoDetailPage PhotoDetailPage => _photoDetailPage ?? throw new NullReferenceException();
        protected PhotoListPage PhotoListPage => _photoListPage ?? throw new NullReferenceException();
        protected IApp App => _app ?? throw new NullReferenceException();

        [SetUp]
        protected virtual void BeforeEachTest()
        {
            _app = AppInitializer.StartApp(Platform);

            _addPhotosPage = new AddPhotosPage(App);
            _photoDetailPage = new PhotoDetailPage(App);
            _photoListPage = new PhotoListPage(App);

            App.Screenshot("App Launched");
        }

        [TearDown]
        protected virtual void AfterEachTest()
        {

        }
    }
}

