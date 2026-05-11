using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.BlogCategoryDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Əlavə olundu

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoryController(IBlogCategoryService _service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous] // Bloq kateqoriyalarını hamı görə bilsin
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous] // Müəyyən bir kateqoriyanı hamı görə bilsin
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")] // Kateqoriya yaratmaq hər ikisinə olar
        public async Task<IActionResult> Create(BlogCategoryCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Kateqoriya yaradildi.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Moderator")] // Redaktə etmək hər ikisinə olar
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