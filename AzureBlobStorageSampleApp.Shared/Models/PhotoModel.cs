using System;

namespace AzureBlobStorageSampleApp.Shared
{
    public class PhotoModel : IBaseModel
    {
        public PhotoModel() => Id = Guid.NewGuid().ToString();

        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
