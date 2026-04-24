using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

  
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.GetAllAsync();
            return Ok(result);
        }

        // ✅ GET: api/teacher/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _teacherService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(result);
        }

 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.CreateAsync(dto);

            return StatusCode(201, "Teacher created successfully");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.UpdateAsync(id, dto);

            return Ok("Teacher updated successfully");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);

            return Ok("Teacher deleted successfully");
        }
    }
}