using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;


namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestResultsController : ControllerBase
{
    private readonly ITestResultService _service;

    public TestResultsController(ITestResultService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(TestResultCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}