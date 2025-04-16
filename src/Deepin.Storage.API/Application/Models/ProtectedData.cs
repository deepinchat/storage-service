namespace Deepin.Storage.API.Application.Models;

public class ProtectedData
{
    public required string Data { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
}
