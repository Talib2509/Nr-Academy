using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // ✅ GET: api/course
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            return Ok(result);
        }

        // ✅ POST: api/course
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseService.CreateAsync(dto);

            return StatusCode(201, "Course created successfully");
        }

        // ✅ PUT: api/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseService.UpdateAsync(id, dto);

            return Ok("Course updated successfully");
        }

        // ✅ DELETE: api/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);

            return Ok("Course deleted successfully");
        }
    }
}