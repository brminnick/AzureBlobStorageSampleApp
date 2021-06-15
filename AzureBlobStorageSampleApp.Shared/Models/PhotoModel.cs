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
    public record PhotoModel : IBaseModel
    {
        public PhotoModel() => Id = Guid.NewGuid().ToString();

#if MOBILE
        [PrimaryKey, Unique]
#else
        [Key]
#endif
        public string Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset UpdatedAt { get; init; }
        public bool IsDeleted { get; init; }
        public string Url { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
    }
}

namespace System.Runtime.CompilerServices
{
    [ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)]
    public record IsExternalInit;
}