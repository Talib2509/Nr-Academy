using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        // "İstifadəçilərin fəaliyyətinə nəzarət etmək" - Admin və Moderator baxa bilsin.
        // Həmçinin hər bir istifadəçi öz profilinə baxa bilməsi üçün "Student, Teacher" əlavə oluna bilər.
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/upload-image")]
    
        [Authorize] // Giriş edən hər kəs (özü üçün) və ya idarəçilər
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            try
            {
                var imageUrl = await _userService.UploadProfileImageAsync(id, file);
                return Ok(new { profileImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}