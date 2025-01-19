using Deepin.Storage.API.Infrastructure.Entitites;
using Microsoft.Extensions.Options;

namespace Deepin.Storage.API.Infrastructure.FileStorage;
public class LocalFileStorageOptions
{
    public required string Root { get; set; }
}
public class LocalFileStorage(IOptions<LocalFileStorageOptions> options) : IFileStorage
{
    private readonly string _rootPath = options.Value.Root;

    public StorageProvider Provider => StorageProvider.Local;

    private string GetFullPath(string relativePath)
    {
        return Path.Combine(_rootPath, relativePath);
    }
    public async Task CreateAsync(FileObject file, Stream stream)
    {
        var fullPath = GetFullPath(file.Path);
        var dir = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var fileInfo = new FileInfo(fullPath);
        using var fs = fileInfo.Create();
        await stream.CopyToAsync(fs);
        await fs.FlushAsync();
    }

    public async Task DeleteAsync(FileObject file)
    {
        var fullPath = GetFullPath(file.Path);
        var fileInfo = new FileInfo(fullPath);
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }
        await Task.CompletedTask;
    }

    public async Task<Stream> GetStreamAsync(FileObject file)
    {
        var fullPath = GetFullPath(file.Path);
        var fileInfo = new FileInfo(fullPath);
        if (!fileInfo.Exists)
            return Stream.Null;
        return await Task.FromResult(fileInfo.OpenRead());
    }

    public Task<bool> ExistsAsync(FileObject file)
    {
        var fullPath = GetFullPath(file.Path);
        var fileInfo = new FileInfo(fullPath);
        return Task.FromResult(fileInfo.Exists);
    }

    public Task<string> BuildFilePath(Guid id, string extension)
    {
        var fileName = $"{id}{extension}";
        var dir = DateTime.UtcNow.ToString("yyyy/MM");
        return Task.FromResult(Path.Combine(dir, fileName));
    }
}
