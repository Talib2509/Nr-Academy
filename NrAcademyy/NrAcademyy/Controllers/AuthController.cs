using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
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
            //var user = await _userManager.FindByEmailAsync(dto.Email);

            //if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            //    return Unauthorized();

            //var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            //var tokens = await _jwtService.GenerateTokensAsync(user, ip);

            //return Ok(tokens);
            var token = await _authService.LoginAsync(dto);
            return Ok(new { Token = token });
        }

    }

}
