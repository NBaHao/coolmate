using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly DBContext _dbContext;

        public UserRepository(UserManager<SiteUser> userManager, DBContext dBContext)
        {
            _userManager = userManager;
            _dbContext = dBContext;
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

        public async Task<bool> UpdateUserInfomationAsync(string userId, string name, string email, string phoneNumber)
        {
            if (name == null || email == null || phoneNumber == null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            if (user.Email != email && await IsUserExists(email))
            {
                return false;
            }

            user.Name = name;
            user.Email = email;
            user.PhoneNumber = phoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) { return true; }
            return false;
        }

        public async Task<bool> AddUserAddressAsync(SiteUser user, string addressLine)
        {
            Address address = await _dbContext.Addresses.FirstOrDefaultAsync(ad => ad.AddressLine == addressLine);
            if (address == null)
            {
                address = new Address { AddressLine = addressLine };
                await _dbContext.Addresses.AddAsync(address);
                await _dbContext.SaveChangesAsync();
            }

            var result = await
                _dbContext.UserAddresses
                    .Where(ua => ua.Address == address && ua.User == user)
                    .FirstOrDefaultAsync();
            if (result == null)
            {
                await _dbContext.UserAddresses.AddAsync(new UserAddress { AddressId = address.Id, UserId = user.Id, IsDefault = null });
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> MakeUserAddressDefaultAsync(string userId, int addressId)
        {
            if (!await _dbContext.Users.AnyAsync(user => user.Id == userId))
            {
                return false;
            }

            var userAddresses
                = await _dbContext.UserAddresses
                .Select(userAddress => userAddress)
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            foreach (var userAddress in userAddresses)
            {
                if (userAddress.AddressId != addressId) { userAddress.IsDefault = null; }
                else userAddress.IsDefault = 1;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<AddressDTO>> GetUserAddressesAsync(SiteUser user)
        {
            var addresses = await _dbContext.UserAddresses
                .Where(ua => ua.UserId == user.Id)
                .Select(ua => new AddressDTO
                {
                    addressId = ua.AddressId ?? 0,
                    streetLine = ua.Address.AddressLine ?? string.Empty,
                    isDefault = ua.IsDefault
                })
                .ToListAsync();

            return addresses;
        }
    }
}
