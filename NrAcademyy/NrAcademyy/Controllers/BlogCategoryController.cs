using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.BlogCategoryDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService _service;

        public BlogCategoryController(IBlogCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(BlogCategoryCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Kateqoriya yaradildi.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(int id, BlogCategoryUpdateDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Kateqoriya yenilendi.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Kateqoriya silindi.");
        }
    }
}