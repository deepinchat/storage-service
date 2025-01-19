using System;
using Deepin.Storage.API.Infrastructure.Entitites;

namespace Deepin.Storage.API.Infrastructure.FileStorage;

public class S3FileStorage : IFileStorage
{
    public StorageProvider Provider => StorageProvider.AmazonS3;

    public Task<string> BuildFilePath(Guid id, string extension)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(FileObject file, Stream stream)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(FileObject file)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(FileObject file)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> GetStreamAsync(FileObject file)
    {
        throw new NotImplementedException();
    }
}
