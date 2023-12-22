using CoolMate.DTO;
using CoolMate.Repositories.Interfaces;
using CoolMate.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CoolMate.Models;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public authController(
            AuthService authService,
            IUserRepository userRepository,
            TokenService tokenService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _tokenService = tokenService;
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

        [HttpGet("google-login")]
        public IActionResult Login()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(HandleGoogleLogin))
            };

            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> HandleGoogleLogin()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                return BadRequest("Google authentication failed.");
            }

            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Check if the user already exists in the database
            var existingUser = await _userRepository.IsUserExists(email);
                
            if (existingUser == false)
            {
                // Create a new user in the database
                var newUser = new SiteUser
                { 
                    Email = email,
                    Name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value
                };

                await _authService.Register(new RegisterDTO { Email = email, Password = "anhyeuem"});
            }

            var res = await _authService.GoogleLogin(email);

            return Ok(res.Data);
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Check if the token is already blacklisted
            var isBlacklisted = await _authService.IsTokenBlacklistedAsync(token);
            if (isBlacklisted)
            {
                return BadRequest("Token already blacklisted");
            }
            TimeSpan timeSpan = _tokenService.GetRemainingTime(token).Value;
            
            await _authService.BlacklistTokenAsync(token, timeSpan);

            return Ok("Logout successful");
        }
    }
}
