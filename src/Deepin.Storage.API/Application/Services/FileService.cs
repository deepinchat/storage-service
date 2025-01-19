using Deepin.Domain;
using Deepin.Storage.API.Application.Models;
using Deepin.Storage.API.Infrastructure;
using Deepin.Storage.API.Infrastructure.Entitites;
using Deepin.Storage.API.Infrastructure.FileStorage;
using Deepin.Storage.API.Infrastructure.Helpers;
using HeyRed.Mime;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Storage.API.Application.Services;

public class FileService(IFileStorage fileStorage, IUserContext userContext, StorageDbContext db) : IFileService
{
    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly IUserContext _userContext = userContext;
    private readonly StorageDbContext _db = db;

    public async Task<FileModel> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        var hash = await CalculateHashAsync(stream, cancellationToken);
        var existingFile = await this.GetByHashAsync(hash, cancellationToken);
        if (existingFile is not null)
        {
            return existingFile;
        }
        var fileId = Guid.NewGuid();
        var path = await _fileStorage.BuildFilePath(fileId, Path.GetExtension(fileName));
        var checksum = await CalculateChecksumAsync(stream, cancellationToken);
        var file = new FileObject(
            provider: _fileStorage.Provider,
            name: fileName,
            path: path,
            hash: hash,
            checksum: checksum,
            format: Path.GetExtension(fileName),
            mimeType: MimeTypesMap.GetMimeType(fileName),
            length: stream.Length,
            createdBy: _userContext.UserId,
            id: fileId);

        await _fileStorage.CreateAsync(file, stream);
        await _db.FileObjects.AddAsync(file);
        await _db.SaveChangesAsync();
        return file.ToModel();
    }
    public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await _db.FileObjects.FirstOrDefaultAsync(f => f.Id == fileId, cancellationToken);
        if (file is null)
        {
            return;
        }
        await _fileStorage.DeleteAsync(file);
        _db.FileObjects.Remove(file);
        await _db.SaveChangesAsync();
    }
    public async Task<Stream?> GetStreamAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await _db.FileObjects.FirstOrDefaultAsync(f => f.Id == fileId, cancellationToken);
        if (file is null)
        {
            return null;
        }
        return await _fileStorage.GetStreamAsync(file);
    }
    public async Task<FileModel?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await _db.FileObjects.FirstOrDefaultAsync(f => f.Id == fileId, cancellationToken);
        return file?.ToModel();
    }
    public async Task<FileModel?> GetByHashAsync(string hash, CancellationToken cancellationToken = default)
    {
        var file = await _db.FileObjects.FirstOrDefaultAsync(f => f.Hash == hash, cancellationToken);
        return file?.ToModel();
    }
    private async Task<string> CalculateHashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return await HashHelper.GetSHA256HashAsync(stream, cancellationToken);
    }

    private async Task<string> CalculateChecksumAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(ChecksumHelper.GetCRC32Checksum(stream, cancellationToken));
    }
}
