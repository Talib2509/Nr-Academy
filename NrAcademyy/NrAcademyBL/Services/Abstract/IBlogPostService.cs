
using NrAcademyBL.DTOs.BlogPostDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IBlogPostService
    {
        Task<List<BlogPostGetDTO>> GetAsync();
        Task<BlogPostGetDTO> GetByIdAsync(int id);
        Task CreateAsync(BlogPostCreateDTO dto, string rootPath);
        Task UpdateAsync(BlogPostUpdateDTO dto, string rootPath);
        Task DeleteAsync(int id, string rootPath);
    }
}