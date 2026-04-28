using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using System;
using System.Threading.Tasks;

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
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> UploadProfileImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Fayl seçilməyib.");

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