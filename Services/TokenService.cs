using CoolMate.Interfaces;
using CoolMate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoolMate.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly SymmetricSecurityKey _key;
        public TokenService(UserManager<SiteUser> userManager, IConfiguration config)
        {
            this._userManager = userManager;
            this._key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
        }
        public async Task<string> CreateToken(SiteUser user)
        {
            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Email.ToString()),
            };
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public DateTime? GetTokenExpiration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jsonToken != null)
                {
                    var expiration = jsonToken.ValidTo;
                    return expiration;
                }
            }
            return null;
        }

        public TimeSpan? GetRemainingTime(string token)
        {
            var expiration = GetTokenExpiration(token);
            if (expiration.HasValue)
            {
                var remainingTime = expiration.Value - DateTime.UtcNow;
                return remainingTime;
            }
            return null;
        }
    }
}
