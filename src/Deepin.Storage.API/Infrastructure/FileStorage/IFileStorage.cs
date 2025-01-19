using Deepin.Storage.API.Infrastructure.Entitites;

namespace Deepin.Storage.API.Infrastructure.FileStorage;

public interface IFileStorage
{
    StorageProvider Provider { get; }
    Task CreateAsync(FileObject file, Stream stream);
    Task DeleteAsync(FileObject file);
    Task<Stream> GetStreamAsync(FileObject file);
    Task<bool> ExistsAsync(FileObject file);
    Task<string> BuildFilePath(Guid id, string extension);
}
