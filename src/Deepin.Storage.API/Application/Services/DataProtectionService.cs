using Deepin.Storage.API.Application.Models;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;

namespace Deepin.Storage.API.Application.Services;

public class DataProtectionService(IDataProtectionProvider dataProtectionProvider) : IDataProtectionService
{
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector(nameof(DataProtectionService));
    public string Protect(string data)
    {
        if (string.IsNullOrEmpty(data)) return string.Empty;
        return this.Protect(new ProtectedData
        {
            Data = data
        });
    }
    public string Protect(string data, DateTime expiresAt)
    {
        if (string.IsNullOrEmpty(data)) return string.Empty;
        return this.Protect(new ProtectedData
        {
            Data = data,
            ExpiresAt = expiresAt
        });
    }
    public string Protect(ProtectedData data)
    {
        if (data is null) return string.Empty;
        return _dataProtector.Protect(JsonConvert.SerializeObject(data));
    }
    public ProtectedData? Unprotect(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        try
        {
            var json = _dataProtector.Unprotect(token);
            return JsonConvert.DeserializeObject<ProtectedData>(json);
        }
        catch (Exception)
        {
            return null;
        }
    }
    public bool Validate(string token)
    {
        if (string.IsNullOrEmpty(token)) return false;
        try
        {
            var json = _dataProtector.Unprotect(token);
            if (string.IsNullOrEmpty(json)) return false;
            var data = JsonConvert.DeserializeObject<ProtectedData>(json);
            if (data is null) return false;
            if (data.ExpiresAt.HasValue && data.ExpiresAt.Value < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
