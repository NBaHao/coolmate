using CoolMate.Models;

namespace CoolMate.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(SiteUser user);
        TimeSpan? GetRemainingTime(string token);
        DateTime? GetTokenExpiration(string token);
    }
}
