using System;

namespace AzureBlobStorageSampleApp.Shared
{
    public interface IBaseModel
    {
        string Id { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}
