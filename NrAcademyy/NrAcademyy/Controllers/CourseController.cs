using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; 
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IWebHostEnvironment _env; 

        public CourseController(ICourseService courseService, IWebHostEnvironment env)
        {
            _courseService = courseService;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseCreateDTO dto) 
        {
            await _courseService.CreateAsync(dto, _env.WebRootPath);
            return StatusCode(201, "Course created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CourseUpdateDTO dto)
        {
            await _courseService.UpdateAsync(id, dto, _env.WebRootPath);
            return Ok("Course updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id, _env.WebRootPath);
            return Ok("Course deleted successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _courseService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _courseService.GetByIdAsync(id));
    }
}