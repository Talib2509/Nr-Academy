using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.TestiomonialDTO;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestimonialsController(ITestimonialService _service) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    
    public async Task<IActionResult> Create([FromBody] TestimonialCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.CreateAsync(dto);
        return StatusCode(201, "Rey uqurla elave edildi");
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] TestimonialUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("URL id ile DTO id uyqun gelmir.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}

