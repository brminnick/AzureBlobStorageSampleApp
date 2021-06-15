using System;

namespace AzureBlobStorageSampleApp.Shared
{
    public interface IBaseModel
    {
        string Id { get; }
        DateTimeOffset UpdatedAt { get; }
        DateTimeOffset CreatedAt { get; }
        bool IsDeleted { get; }
    }
}
