using System;
#if BACKEND
using System.Data.Linq.Mapping;
#elif MOBILE
using SQLite;
using Newtonsoft.Json;
#endif

namespace AzureBlobStorageSampleApp.Shared
{
#if BACKEND
    [Table(Name = "PhotoModels")]
#endif
    public class PhotoModel : IBaseModel
    {
        public PhotoModel() => Id = Guid.NewGuid().ToString();

#if BACKEND
        [Column(Name = nameof(Id), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#elif MOBILE
        [PrimaryKey, Unique]
#endif
        public string Id { get; set; }

#if BACKEND
        [Column(Name = nameof(CreatedAt), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#endif
        public DateTimeOffset CreatedAt { get; set; }

#if BACKEND
        [Column(Name = nameof(UpdatedAt), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#endif
        public DateTimeOffset UpdatedAt { get; set; }

#if BACKEND
        [Column(Name = nameof(IsDeleted), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#endif
        public bool IsDeleted { get; set; }

#if BACKEND
        [Column(Name = nameof(Url), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#endif
        public string Url { get; set; }

#if BACKEND
        [Column(Name = nameof(Title), IsPrimaryKey = true, CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
#endif
        public string Title { get; set; }
    }
}
