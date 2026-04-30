using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
// Test suallarını idarə etmək üçün Admin və ya Moderator olmaq lazımdır
[Authorize(Roles = "Admin, Moderator")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;

    public QuestionsController(IQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    // Sualların siyahısına hər iki rol baxa bilər
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    // "Sualları hazırlamaq" - Həm Admin, həm də Moderator üçün icazəlidir
    public async Task<IActionResult> Create(QuestionCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    // Mövcud sualları redaktə etmək hər iki rola icazəlidir
    public async Task<IActionResult> Update(QuestionUpdateDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    // Sənin qaydana əsasən, mühüm datanı (sual bazasını) silmək YALNIZ Adminə məxsusdur
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}