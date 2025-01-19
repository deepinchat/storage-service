using System.Security.Cryptography;

namespace Deepin.Storage.API.Infrastructure.Helpers;

public class HashHelper
{
    public async Task<bool> VerifyFileIntegrityByHash(Stream fileStream, string originalHash)
    {
        var currentHash = await GetSHA256HashAsync(fileStream);
        return string.Equals(currentHash, originalHash, StringComparison.OrdinalIgnoreCase);
    }

    public static async Task<string> GetSHA256HashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        stream.Position = 0;
        using var sha256 = SHA256.Create();
        var hash = await sha256.ComputeHashAsync(stream, cancellationToken);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
