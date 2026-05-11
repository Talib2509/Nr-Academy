using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using static NrAcademyBL.DTOs.TestDTO.TestDTO;
using NrAcademyBL.DTOs.TestDTO;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
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
    [Authorize(Roles = "Admin, Moderator, Teacher")] 
    public async Task<IActionResult> Create(TestCreateDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    [HttpPut]
    [Authorize(Roles = "Admin, Moderator, Teacher")] 
    public async Task<IActionResult> Update(TestUpdateDto dto)
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