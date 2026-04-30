using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

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
        [AllowAnonymous] // Sayta daxil olanlar müəllim siyahısını görə bilsin
        public async Task<IActionResult> GetAll()
        {
            var result = await _teacherService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Müəllimin profil detalları hər kəsə açıq olsun
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _teacherService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(result);
        }

        [HttpPost]
        // Sənin qaydana görə: "Yeni müəllim hesabı yaratmaq" YALNIZ Adminə məxsusdur
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] TeacherCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.CreateAsync(dto);

            return StatusCode(201, "Teacher created successfully");
        }

        [HttpPut("{id}")]
        // "Müəllim Profilləri: Bioqrafiyanı, şəkilləri və təcrübəni yeniləmək" - Admin və Moderator
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(int id, [FromBody] TeacherUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _teacherService.UpdateAsync(id, dto);

            return Ok("Teacher updated successfully");
        }

        [HttpDelete("{id}")]
        // Sistemdən müəllimin silinməsi mühüm əməliyyatdır - YALNIZ Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteAsync(id);

            return Ok("Teacher deleted successfully");
        }
    }
}