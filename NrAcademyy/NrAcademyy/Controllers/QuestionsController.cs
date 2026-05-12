using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; 

using NrAcademyBL.DTOs.QuestionDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]

[Authorize(Roles = "Admin, Moderator,Teacher")]
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
    
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}