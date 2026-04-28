using Microsoft.AspNetCore.Http;

namespace NrAcademyBL.DTOs.BlogPostDTO
{
    public class BlogPostCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; } 
    }

   
}