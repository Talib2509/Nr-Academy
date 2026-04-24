using NrAcademyBL.DTOs.BlogCategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract.BlogCategoryAbstract;

public interface IBlogCategoryService
{
    Task<IEnumerable<BlogCategoryGetDTO>> GetAllAsync();
    Task<BlogCategoryGetDTO> GetByIdAsync(int id);
    Task CreateAsync(BlogCategoryCreateDTO dto);
    Task UpdateAsync(int id, BlogCategoryUpdateDTO dto);
    Task DeleteAsync(int id);
}
