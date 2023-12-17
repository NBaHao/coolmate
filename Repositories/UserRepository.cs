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

        public async Task<bool> UpdateUserInfomationAsync(string userId, string name, string email, string phoneNumber, string birthday, string gender, int? height, int? weight)
        {
            if (name == null || email == null || phoneNumber == null ) return false;

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
            user.Birthday = birthday;
            user.Gender = gender;
            user.Weight = weight;
            user.Height = height;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) { return true; }
            return false;
        }

        public async Task<bool> AddUserAddressAsync(SiteUser user, AddressDTO addressDTO)
        {
            Address address = await _dbContext.Addresses.FirstOrDefaultAsync(ad => ad.AddressLine == addressDTO.streetLine);
            if (address == null)
            {
                address = new Address { AddressLine = addressDTO.streetLine };
                await _dbContext.Addresses.AddAsync(address);
                await _dbContext.SaveChangesAsync();
            }

            var result = await
                _dbContext.UserAddresses
                    .Where(ua => ua.Address == address && ua.User == user)
                    .FirstOrDefaultAsync();
            if (result == null)
            {
                await _dbContext.UserAddresses.AddAsync(
                    new UserAddress { 
                        AddressId = address.Id,
                        UserId = user.Id, 
                        IsDefault = null,
                        Name = addressDTO.Name, 
                        PhoneNumber = addressDTO.PhoneNumber });
                await _dbContext.SaveChangesAsync();

                if (addressDTO.isDefault == 1)
                    await MakeUserAddressDefaultAsync(user.Id, address.Id);
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
                    isDefault = ua.IsDefault,
                    PhoneNumber = ua.PhoneNumber ?? string.Empty,
                    Name = ua.Name ?? string.Empty,
                })
                .ToListAsync();

            return addresses;
        }
        public async Task<bool> DeleteUserAddressAsync(SiteUser user, int addressId)
        {
            var userAddress = await _dbContext.UserAddresses
                .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.AddressId == addressId);

            if (userAddress != null)
            {
                _dbContext.UserAddresses.Remove(userAddress);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateUserAddressAsync(SiteUser user, AddressDTO updatedAddress)
        {
            var userAddress = await _dbContext.UserAddresses.Include(ua => ua.Address)
                .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.AddressId == updatedAddress.addressId);

            if (userAddress != null)
            {
                userAddress.Name = updatedAddress.Name;
                userAddress.PhoneNumber = updatedAddress.PhoneNumber;
                userAddress.Address.AddressLine = updatedAddress.streetLine;

                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> isAdmin(SiteUser user)
        {
            //check if user have role admin
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
        public async Task<IEnumerable<SiteUser>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}
