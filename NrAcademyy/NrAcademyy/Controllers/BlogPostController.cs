using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController(IBlogPostService _service) : ControllerBase
    {

        [HttpGet]
        [AllowAnonymous] // Bloq yazıları hər kəs tərəfindən oxuna bilsin
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Yazının detalı hər kəs üçün açıq olsun
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _service.GetByIdAsync(id);
            return Ok(post);
        }

        [HttpPost]
        // "Yeni məqalələr yazmaq" - Moderator və Admin üçün
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Post(BlogPostCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        // "SEO üçün linkləri tənzimləmək və məzmunu yeniləmək" - Moderator və Admin üçün
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Put(BlogPostUpdateDTO dto, int id)
        {
            dto.Id = id;
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        // "Sistemdən mühüm məlumatların tam silinməsi" - YALNIZ Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}