using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.ForgotPassword;
using NrAcademyBL.Extensions;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Enums;

using System.Data;

namespace NrAcademyBL.Services.Concrete
{
    public class AuthService(UserManager<AppUser> _userManager, IMapper _mapper, IJwtService _jwtService, IEmailService _emailService) : IAuthService
    {
        public async Task<AuthResponseDto> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                throw new System.Exception("Istifadeci tapilmadi");
            }
            var isTruePassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isTruePassword)
            {
                throw new System.Exception("Email ve ya password duzgun deyil");

            }
            if (!user.IsEmailVerified)
            {
                throw new Exception("Email tesdiqlenmeyib");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var authResponse = await _jwtService.GenerateTokensAsync(user, roles.FirstOrDefault());

            return authResponse;

        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDTO dto)
        {

            if (dto.Role != Roles.Teacher && dto.Role != Roles.Student)
                throw new Exception("Yalniz Teacher ve ya Student secile biler");

            var isExistEmail = await _userManager.Users
                .AnyAsync(x => x.Email.ToLower() == dto.Email.ToLower());

            if (isExistEmail)
                throw new Exception("Bu Email artiq movcuddur");

            var appUser = _mapper.Map<AppUser>(dto);
            appUser.UserName = dto.Email;

        
            var verificationCode = GenerateVerificationCode();
            appUser.EmailVerificationCode = verificationCode;
            appUser.EmailVerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            appUser.IsEmailVerified = false;

            var result = await _userManager.CreateAsync(appUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            await _userManager.AddToRoleAsync(user, dto.Role.GetRole());

            var isSent = await _emailService.SendVerificationEmailAsync(user.Email, verificationCode);

            if (!isSent)
            {
                throw new Exception("Email gonderile bilmedi");
            }

            return new();
        }
        public async Task VerifyEmailAsync(VerifyEmailDTO dto)
        {
            // Email-ə görə istifadəçini tapırıq
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("İstifadəçi tapılmadı");

            // Əgər istifadəçi artıq təsdiqlənibsə, boşuna update etməyək
            if (user.IsEmailVerified)
                throw new Exception("Bu hesab artıq təsdiqlənib");

            // Kodun null olub-olmadığını yoxlayaq (məsələn, kimsə kod göndərmədən doğrulamağa çalışarsa)
            if (string.IsNullOrEmpty(user.EmailVerificationCode))
                throw new Exception("Aktiv təsdiqləmə kodu tapılmadı");

            // Kodun doğruluğunu yoxlayırıq
            if (user.EmailVerificationCode != dto.Code)
                throw new Exception("Kod yanlışdır");

            // Vaxtın bitib-bitmədiyini yoxlayırıq
            if (user.EmailVerificationCodeExpiry.HasValue && user.EmailVerificationCodeExpiry.Value < DateTime.UtcNow)
                throw new Exception("Kodun vaxtı bitmişdir");

            // Məntiqi olaraq məlumatları yeniləyirik
            user.IsEmailVerified = true;
            user.EmailVerificationCode = null;        // Artıq AppUser-də string? olduğu üçün xəta verməyəcək
            user.EmailVerificationCodeExpiry = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Update error: {errors}");
            }
        }
        public async Task ForgotPasswordAsync(ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return; // security: user exists/doesn't exist reveal etmə

            var code = GenerateVerificationCode();

            user.PasswordResetCode = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            await _emailService.SendVerificationEmailAsync(user.Email, code);
        }
        public async Task VerifyResetCodeAsync(VerifyResetCodeDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid request");

            if (user.PasswordResetCode != dto.Code)
                throw new Exception("Code is incorrect");

            if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
                throw new Exception("Code expired");
        }
        public async Task ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Invalid request");

            if (user.PasswordResetCode != dto.Code)
                throw new Exception("Invalid code");

            if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
                throw new Exception("Code expired");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new Exception(errors);
            }

            // cleanup
            user.PasswordResetCode = null;
            user.PasswordResetCodeExpiry = null;

            await _userManager.UpdateAsync(user);
        }
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 rakamlı kod
        }
    }
}
