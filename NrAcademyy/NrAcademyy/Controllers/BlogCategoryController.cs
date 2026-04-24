using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract.BlogCategoryAbstract;

namespace NrAcademyy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogCategoryController(IBlogCategoryService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Create(BlogCategoryCreateDTO dto)
    {
        await _service.CreateAsync(dto);
        return Ok("Kateqoriya yaradildi.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BlogCategoryUpdateDTO dto)
    {
        await _service.UpdateAsync(id, dto);
        return Ok("Kateqoriya yenilendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok("Kateqoriya silindi.");
    }
}
