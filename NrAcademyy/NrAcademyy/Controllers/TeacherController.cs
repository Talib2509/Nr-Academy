using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IWebHostEnvironment _env;

        public TeacherController(ITeacherService teacherService, IWebHostEnvironment env)
        {
            _teacherService = teacherService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _teacherService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _teacherService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TeacherCreateDTO dto)
        {
            await _teacherService.CreateAsync(dto, _env.WebRootPath);
            return StatusCode(201, "Teacher created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] TeacherUpdateDTO dto)
        {
            await _teacherService.UpdateAsync(id, dto, _env.WebRootPath);
            return Ok("Teacher updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id, _env.WebRootPath);
            return Ok("Teacher deleted successfully");
        }
    }
}