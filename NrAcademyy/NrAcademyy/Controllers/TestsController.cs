using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir
using static NrAcademyBL.DTOs.TestDTO.TestDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
// Testlərin idarə olunması üçün Admin və ya Moderator olmaq mütləqdir
[Authorize(Roles = "Admin, Moderator")]
public class TestsController : ControllerBase
{
    private readonly ITestService _service;

    public TestsController(ITestService service)
    {
        _service = service;
    }

    [HttpGet]
    // Həm Admin, həm də Moderator testlərin siyahısını görə bilsin
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    // "Testləri hazırlamaq və vaxt təyin etmək" - Moderator və Admin üçün icazəlidir
    public async Task<IActionResult> Create(TestCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    // Testlərin parametrlərini (məsələn, vaxtını və ya adını) hər iki rol dəyişə bilər
    public async Task<IActionResult> Update(TestUpdateDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    // "Sistemdən mühüm məlumatların tamamilə silinməsi" - YALNIZ Admin.
    // Bütöv bir testin silinməsi ona bağlı olan bütün sualları və nəticələri itirə bilər.
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}