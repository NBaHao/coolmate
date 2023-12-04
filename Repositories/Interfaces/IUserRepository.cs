using CoolMate.DTO;
using CoolMate.Models;
using Microsoft.AspNetCore.Identity;

namespace CoolMate.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUserExists(string email);
        Task<IdentityResult> CreateUserAsync(SiteUser user, string password);
        Task<IdentityResult> AddToRoleAsync(SiteUser user, string role);
        Task<SiteUser> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(SiteUser user, string password);
        Task<IdentityResult> ChangePasswordAsync(SiteUser user, string oldPassword, string newPassword);
        Task<SiteUser> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(SiteUser user);
        Task UpdateSecurityStampAsync(SiteUser user);
        Task<IdentityResult> ResetPasswordAsync(SiteUser user, string token, string newPassword);
        Task<bool> UpdateUserInfomationAsync(string userId, string name, string email, string phoneNumber, string birthday, string gender, int? height, int? weight);
        Task<bool> AddUserAddressAsync(SiteUser user, AddressDTO addressDTO);
        Task<bool> MakeUserAddressDefaultAsync(string userId, int addressId);
        Task<List<AddressDTO>> GetUserAddressesAsync(SiteUser user);
        Task<bool> DeleteUserAddressAsync(SiteUser user, int addressId);
        Task<bool> UpdateUserAddressAsync(SiteUser user, AddressDTO updatedAddress);
        Task<bool> isAdmin(SiteUser user);
    }
}
