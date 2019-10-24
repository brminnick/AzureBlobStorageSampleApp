namespace AzureBlobStorageSampleApp.Shared
{
    public class PhotoBlobModel
    {
        public PhotoBlobModel(byte[] image) => Image = image;

        public byte[] Image { get; }
    }
}
