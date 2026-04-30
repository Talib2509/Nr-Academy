using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.EnrollmentDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Qeydiyyat məlumatları daxili idarəetmə olduğu üçün ümumi icazə tələb edirik
    [Authorize(Roles = "Admin, Moderator")]
    public class EnrollmentsController(IEnrollmentService _service) : ControllerBase
    {
        [HttpGet]
        // Həm Admin, həm Moderator siyahıya baxa bilər
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        // Kursa yazılma (qeydiyyat) yaratmaq hər iki idarəçi tərəfindən edilə bilər
        public async Task<IActionResult> Create(EnrollmentCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Qeydiyyat yaradıldı.");
        }

        [HttpPut]
        // "Kurslara yazılma statusuna nəzarət etmək" (məsələn: aktiv/passiv etmək)
        public async Task<IActionResult> Update(EnrollmentUpdateDTO dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Qeydiyyat yeniləndi.");
        }

        [HttpDelete("{id}")]
        // Qeydiyyatın silinməsi mühüm məlumat itkisidir - YALNIZ Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Qeydiyyat silindi.");
        }
    }
}