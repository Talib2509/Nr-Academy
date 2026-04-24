using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.EnrollmentDTO;
using NrAcademyBL.Services.Abstract.EnrollmentAbstract;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentsController(IEnrollmentService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(EnrollmentCreateDTO dto)
    {
        await _service.CreateAsync(dto);
        return Ok("Qeydiyyat yaradıldı.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(EnrollmentUpdateDTO dto)
    {
        await _service.UpdateAsync(dto);
        return Ok("Qeydiyyat yenilendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Qeydiyyat silindi.");
    }
}
