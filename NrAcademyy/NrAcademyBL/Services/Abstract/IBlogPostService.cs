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

        Task CreateAsync(BlogPostCreateDTO dto);
        Task UpdateAsync(BlogPostUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
