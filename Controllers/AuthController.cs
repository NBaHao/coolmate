using CoolMate.DTO;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public authController(
            AuthService authService,
            IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var response = await _authService.Register(registerDto);
            if (response.Errors == null)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Errors);
        }

        [HttpPost("addrole")]
        public async Task<ActionResult> Addrole(string email, string role)
        {
            var res = await _authService.Addrole(email, role);
            return Ok(res);
        }

        [HttpPost("createrole")]
        public async Task<ActionResult> createroll(string role)
        {
            var res = await _authService.createroll(role);
            return Ok(res);

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var res = await _authService.Login(loginDTO);
            if (res.Errors == null)
            {
                return Ok(res.Data);
            }
            else
            {
                if (res.ErrorCode == 401) return Unauthorized(res.Errors);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
        {
            var user = await _userRepository.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var res = await _authService.ChangePassword(user, changePasswordDto);
            if (res.Errors == null)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Errors);
            }
        }

        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            var res = await _authService.ForgotPassword(forgotPasswordDto);
            if (res.Errors != null)
            {
                if (res.ErrorCode == 404) return NotFound(res.Errors);
            } else
            {
                return Ok(res.Data);
            }
            return BadRequest();
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            var res = await _authService.ResetPassword(resetPasswordDto);
            if (res.Errors != null)
            {
                if (res.ErrorCode == 404) return NotFound(res.Errors);
            }
            else
            {
                return Ok(res.Data);
            }
            return BadRequest();
        }
    }
}
