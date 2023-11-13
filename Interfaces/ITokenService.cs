using CoolMate.Models;

namespace CoolMate.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(SiteUser user);
    }
}
