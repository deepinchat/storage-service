using Deepin.Storage.API.Infrastructure.Entitites;

namespace Deepin.Storage.API.Application.Models;

public class FileModel
{
    public Guid Id { get; set; }
    public string Hash { get; set; }
    public string Checksum { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Format { get; set; }
    public string MimeType { get; set; }
    public long Length { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public StorageProvider Provider { get; set; }
    public FileModel()
    {
        Name = string.Empty;
        Path = string.Empty;
        Checksum = string.Empty;
        Hash = string.Empty;
        Format = string.Empty;
        MimeType = string.Empty;
        CreatedBy = string.Empty;
    }
}
