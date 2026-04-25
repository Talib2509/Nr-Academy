using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using static NrAcademyBL.DTOs.AuthDTO.TestDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    private readonly ITestService _service;

    public TestsController(ITestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(TestCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TestUpdateDto dto)
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