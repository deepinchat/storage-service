using Deepin.Storage.API.Application.Models;

namespace Deepin.Storage.API.Application.Services;
public interface IFileService
{
    Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<FileModel?> GetByHashAsync(string hash, CancellationToken cancellationToken = default);
    Task<FileModel?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<Stream?> GetStreamAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<FileModel> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default);
}