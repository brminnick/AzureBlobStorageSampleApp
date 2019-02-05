using System;
#if BACKEND
using NPoco;
#elif MOBILE
using SQLite;
#endif

namespace AzureBlobStorageSampleApp.Shared
{
#if BACKEND
    [TableName("PhotoModels"), PrimaryKey(nameof(Id), AutoIncrement = false)]
#endif
    public class PhotoModel : IBaseModel
    {
        public PhotoModel() => Id = Guid.NewGuid().ToString();

#if MOBILE
        [PrimaryKey, Unique]
#endif
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
