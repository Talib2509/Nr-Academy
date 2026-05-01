
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;
using System.Threading.Tasks;

namespace NrAcademyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeacherController(ITeacherService teacherService, IWebHostEnvironment webHostEnvironment)
        {
            _teacherService = teacherService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _teacherService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] TeacherCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.CreateAsync(dto, _webHostEnvironment.WebRootPath);

            return StatusCode(201, "Teacher created successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(int id, [FromForm] TeacherUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.UpdateAsync(id, dto, _webHostEnvironment.WebRootPath);

            return Ok("Teacher updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id, _webHostEnvironment.WebRootPath);

            return Ok("Teacher deleted successfully");
        }
    }
}