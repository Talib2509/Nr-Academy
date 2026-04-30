using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

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
        [AllowAnonymous] // Kursları hər kəs görə bilsin və süzgəcləyə bilsin
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Kursun detallarına hamı baxa bilsin
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            return Ok(result);
        }

        // ✅ POST: api/course
        [HttpPost]
        // Sənin qaydana görə Admin kursları əlavə edir
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CourseCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseService.CreateAsync(dto);

            return Ok("Course created successfully");
        }

        // ✅ PUT: api/course/{id}
        [HttpPut("{id}")]
        // Moderator kurs detallarını (ad, təsvir, qiymət, səviyyə) yeniləyə bilər
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _courseService.UpdateAsync(id, dto);

            return Ok("Course updated successfully");
        }

        // ✅ DELETE: api/course/{id}
        [HttpDelete("{id}")]
        // "Sistemdən mühüm məlumatların tamamilə silinməsi" - YALNIZ Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseService.DeleteAsync(id);

            return Ok("Course deleted successfully");
        }
    }
}