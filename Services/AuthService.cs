using AutoMapper;
using CoolMate.DTO;
using CoolMate.Helpers;
using CoolMate.Interfaces;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WebApplication1.Wrappers;

namespace CoolMate.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<IdentityRole> _roleManager; /////// xong thì xóa những gì liên quan tới role
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        public AuthService(IUserRepository userRepository,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IMapper mapper,
            IEmailService EmailService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = EmailService;
        }

        public async Task<Response<string>> Register(RegisterDTO registerDto)
        {
            var response = new Response<string>();
            if (await _userRepository.IsUserExists(registerDto.Email))
            {
                response.Errors = new string[] { "Email is already taken" };
                return response;
            }

            var user = _mapper.Map<SiteUser>(registerDto);
            var result = await _userRepository.CreateUserAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userRepository.AddToRoleAsync(user, "User");
                response.Data = "Registered successfully";
                return response;
            }
            response.Errors = new string[] { result.Errors.ToString() };
            return response;
        }
        public async Task<Response<string>> Addrole(string email, string role)
        {
            var response = new Response<string>();
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                response.Errors = new string[] { "User not found" };
                return response;
            }
            await _userRepository.AddToRoleAsync(user, role);
            response.Data = "successfully";
            return response;
        }
        public async Task<Response<string>> createroll(string role)
        {
            var response = new Response<string>();
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            else
            {
                response.Errors = new string[] { "role already exists" };
                return response;
            }
            response.Data = "successfully";
            return response;
        }
        public async Task<Response<string>> Login(LoginDTO loginDTO)
        {
            var response = new Response<string>();
            var user = await _userRepository.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                response.Errors = new string[] { "incorrect email/password" };
                response.ErrorCode = 401;
                return response;
            }
            var result = await _userRepository.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                response.Errors = new string[] { "incorrect email/password" };
                response.ErrorCode = 401;
                return response;
            }
            response.Data = await _tokenService.CreateToken(user);
            return response;
        }

        public async Task<Response<string>> ChangePassword(SiteUser user, ChangePasswordDTO changePasswordDto)
        {
            var response = new Response<string>();

            var resultCheckPassword = await _userRepository.CheckPasswordAsync(user, changePasswordDto.OldPassword);
            if (!resultCheckPassword)
            {
                response.Errors = new string[] { "incorrect old password" };
                return response;
            }

            var result = await _userRepository.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                response.Data = "Password changed successfully";
                return response;
            }
            else
            {
                response.Errors = new string[] { result.Errors.ToString() };
                return response;
            }
        }
        public async Task<Response<string>> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
        {
            var response = new Response<string>();
            var user = await _userRepository.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                response.Errors = new string[] { "User not found" };
                response.ErrorCode = 404;
                return response;
            }

            await _userRepository.GeneratePasswordResetTokenAsync(user);
            await _userRepository.UpdateSecurityStampAsync(user);

            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);

            //send email
            var resetLink = $"https://your-website.com/resetpassword?email={Uri.EscapeDataString(forgotPasswordDto.Email)}&token={token}";
            var emailBody = $"Click <a href=\"{resetLink}\">here</a> to reset your password.";

            Mailrequest mailrequest = new Mailrequest();
            mailrequest.ToEmail = forgotPasswordDto.Email;
            mailrequest.Subject = "Reset Password Request";
            mailrequest.Body = emailBody;
            await _emailService.SendEmailAsync(mailrequest);

            response.Data = "Password reset token sent successfully/" + token;
            return response;
        }
        public async Task<Response<string>> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            var response = new Response<string>();
            var user = await _userRepository.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                response.Errors = new string[] { "User not found" };
                response.ErrorCode = 404;
                return response;
            }

            var result = await _userRepository.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                response.Data = "Password reset successfully";
                return response;
            }
            else
            {
                response.Errors = new string[] { result.Errors.ToString() };
                return response;
            }
        }
    }
}
