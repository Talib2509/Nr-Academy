using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Services.Abstract;
using System.Threading.Tasks;

namespace NrAcademyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IWebHostEnvironment _env;

        public BlogPostsController(IBlogPostService blogPostService, IWebHostEnvironment env)
        {
            _blogPostService = blogPostService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _blogPostService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _blogPostService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogPostCreateDTO dto)
        {
            await _blogPostService.CreateAsync(dto, _env.WebRootPath);
            return StatusCode(201, "Blog post uğurla yaradıldı");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BlogPostUpdateDTO dto)
        {
            dto.Id = id; // URL-dəki ID-ni DTO-ya mənimsədirik
            await _blogPostService.UpdateAsync(dto, _env.WebRootPath);
            return Ok("Blog post uğurla yeniləndi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogPostService.DeleteAsync(id, _env.WebRootPath);
            return Ok("Blog post silindi");
        }
    }
}