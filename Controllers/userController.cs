using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class userController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public userController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet()]
        [Authorize]
        public async Task<ActionResult> GetUserInfo()
        {
            var user = await _userRepository.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)?.Value);
            if (user == null) { return BadRequest("user not found"); }
            var res = new UserInfoDTO { Email = user.Email, Name = user.Name, PhoneNumber = user.PhoneNumber, Username = user.UserName};
            return Ok(res);
        }

        [HttpPut("updateInfo")]
        [Authorize]
        public async Task<ActionResult> UpdateInfo([FromBody] UpdateUserDTO updateUserDTO)
        {
            var res = await _userRepository.UpdateUserInfomationAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, updateUserDTO.name, updateUserDTO.email, updateUserDTO.phoneNumber);
            if (res) return Ok(res);
            return BadRequest(res);
        }

        [HttpGet("getAdresses")]
        [Authorize]
        public async Task<ActionResult> GetUserAddresses()
        {
            SiteUser user = await _userRepository.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)?.Value);

            var res = await _userRepository.GetUserAddressesAsync(user);
            
            return Ok(res); 
        }

        [HttpPost("addAddress")]
        [Authorize]
        public async Task<ActionResult> AddAdress([FromBody] string addressline)
        {
            SiteUser user = await _userRepository.FindByEmailAsync(User.FindFirst(ClaimTypes.Name)?.Value);
            var res = await _userRepository.AddUserAddressAsync(user, addressline);
            if (res) return Ok(res);
            return BadRequest(res);
        }

        [HttpPost("makeAddressDefault")]
        [Authorize]
        public async Task<ActionResult> MakeUserAddressDefault([FromBody] int addressId )
        {
            var res = await _userRepository.MakeUserAddressDefaultAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, addressId);
            if (res) return Ok(res);
            return BadRequest(res);
        }
    }
}
