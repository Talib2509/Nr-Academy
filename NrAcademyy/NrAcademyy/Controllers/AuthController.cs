using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.ForgotPassword;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities.Identity;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        public AuthController(UserManager<AppUser> userManager,
                              IJwtService jwtService, IAuthService authService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
         
            var token = await _authService.LoginAsync(dto);
            return Ok(new { Token = token });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
        {
            await _authService.ForgotPasswordAsync(dto);
            return Ok("Check email");
        }
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyCode(VerifyResetCodeDTO dto)
        {
            await _authService.VerifyResetCodeAsync(dto);
            return Ok("Code valid");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok("Password changed");
        }

    }

}
