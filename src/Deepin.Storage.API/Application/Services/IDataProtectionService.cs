using Deepin.Storage.API.Application.Models;

namespace Deepin.Storage.API.Application.Services;

public interface IDataProtectionService
{
    string Protect(string data);
    string Protect(string data, DateTime expiresAt);
    string Protect(ProtectedData data);
    ProtectedData? Unprotect(string token);
    bool Validate(string token);
}
