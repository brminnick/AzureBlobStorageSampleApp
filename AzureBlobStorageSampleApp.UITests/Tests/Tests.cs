using NUnit.Framework;

using Xamarin.UITest;

namespace AzureBlobStorageSampleApp.UITests
{
    public class Tests : BaseTest
    {
        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void AppLaunches()
        {

        }

        [TestCase("Punday")]
        [TestCase("Dog Toy")]
        [TestCase("Brandon")]
        public void VerifyPhoto(string photoTitle)
        {
            //Arrange

            //Act
            PhotoListPage.SelectPhoto(photoTitle);
            PhotoDetailPage.WaitForImageToAppear();

            //Assert
            Assert.AreEqual(photoTitle, PhotoDetailPage.PhotoTitle);
        }
    }
}
