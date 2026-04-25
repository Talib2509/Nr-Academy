using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _service;

    public QuestionsController(IQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(QuestionCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> Update(QuestionUpdateDto dto)
    {
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}