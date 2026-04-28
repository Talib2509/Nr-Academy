using NrAcademyBL.DTOs.BlogPostDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostGetDTO>> GetAsync();
        Task<BlogPostGetDTO> GetByIdAsync(int id);
        Task CreateAsync(BlogPostCreateDTO dto, string rootPath); 
        Task UpdateAsync(BlogPostUpdateDTO dto, string rootPath); 
        Task DeleteAsync(int id, string rootPath);
    }
}
