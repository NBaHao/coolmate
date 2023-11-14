using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<SiteUser> _userManager;

        public UserRepository(UserManager<SiteUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsUserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        public async Task<IdentityResult> CreateUserAsync(SiteUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(SiteUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<SiteUser> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> CheckPasswordAsync(SiteUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(SiteUser user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<SiteUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(SiteUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task UpdateSecurityStampAsync(SiteUser user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(SiteUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
