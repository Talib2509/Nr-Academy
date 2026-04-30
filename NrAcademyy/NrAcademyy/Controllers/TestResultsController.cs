using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
// Test nəticələri gizli məlumat olduğu üçün ən azı Moderator və ya Admin olmalıdır
[Authorize(Roles = "Admin, Moderator")]
public class TestResultsController : ControllerBase
{
    private readonly ITestResultService _service;

    public TestResultsController(ITestResultService service)
    {
        _service = service;
    }

    [HttpGet]
    // "Nəticələrin Monitorinqi": Moderator və Admin tələbələrin ballarına baxa bilsin
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost]
    // Test bitdikdə nəticənin yaradılması (Sistem tərəfindən və ya idarəçi tərəfindən)
    public async Task<IActionResult> Create(TestResultCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpDelete("{id}")]
    // "Sistemdən mühüm məlumatların tamamilə silinməsi" - YALNIZ Admin. 
    // Test nəticələrini silmək statistikaya birbaşa təsir edir.
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}