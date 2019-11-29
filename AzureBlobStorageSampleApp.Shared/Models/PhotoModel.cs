using System;
#if BACKEND
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#elif MOBILE
using SQLite;
#endif

namespace AzureBlobStorageSampleApp.Shared
{
#if BACKEND
    [Table("PhotoModels")]
#endif
    public class PhotoModel : IBaseModel
    {
        public PhotoModel() => Id = Guid.NewGuid().ToString();

#if MOBILE
        [PrimaryKey, Unique]
#else
        [Key]
#endif
        public string Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
