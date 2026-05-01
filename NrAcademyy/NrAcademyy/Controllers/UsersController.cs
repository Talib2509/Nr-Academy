
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using System.Threading.Tasks;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost("{id}/upload-image")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            var imageUrl = await _userService.UploadProfileImageAsync(id, file, _webHostEnvironment.WebRootPath);
            return Ok(new { profileImageUrl = imageUrl });
        }
    }
}