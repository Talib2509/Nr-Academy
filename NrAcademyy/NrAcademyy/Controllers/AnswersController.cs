using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir
using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
// Ümumi olaraq həm Admin, həm də Moderator bura daxil ola bilsin
[Authorize(Roles = "Admin, Moderator")]
public class AnswersController : ControllerBase
{
    private readonly IAnswerService _service;

    public AnswersController(IAnswerService service)
    {
        _service = service;
    }

    [HttpGet]
    // Cavab siyahısına baxmaq hər iki rola icazəlidir
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    // Testləri hazırlamaq hər iki rola icazəlidir
    public async Task<IActionResult> Create(AnswerCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    // Redaktə etmək hər iki rola icazəlidir
    public async Task<IActionResult> Update(AnswerUpdateDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}