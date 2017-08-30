using System;

namespace AzureBlobStorageSampleApp
{
    public interface IBaseModel
    {
        string Id { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        bool IsDeleted { get; set; }
    }
}
