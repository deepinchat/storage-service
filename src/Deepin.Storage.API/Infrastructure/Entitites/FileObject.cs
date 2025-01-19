namespace Deepin.Storage.API.Infrastructure.Entitites;
public class FileObject
{
    public Guid Id { get; private set; }
    public string Hash { get; private set; }
    public string Checksum { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    public string Format { get; private set; }
    public string MimeType { get; private set; }
    public long Length { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public StorageProvider Provider { get; private set; }
    public FileObject()
    {
        Name = string.Empty;
        Path = string.Empty;
        Checksum = string.Empty;
        Hash = string.Empty;
        Format = string.Empty;
        MimeType = string.Empty;
        CreatedBy = string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public FileObject(StorageProvider provider, string name, string path, string hash, string checksum, string format, string mimeType, long length, string createdBy, Guid? id = null) : this()
    {
        Id = id ?? Guid.NewGuid();
        Provider = provider;
        Hash = hash;
        Checksum = checksum;
        Name = name;
        Path = path;
        Format = format;
        MimeType = mimeType;
        Length = length;
        CreatedBy = createdBy;
    }
}
