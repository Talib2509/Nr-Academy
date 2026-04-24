using NrAcademyBL.DTOs.BlogPostDTO;

namespace NrAcademyBL.Services.Abstract.BlogPostAbstract;

public interface IBlogPostService
{
    Task<IEnumerable<BlogPostGetDTO>>GetAsync();
    Task<BlogPostGetDTO> GetByIdAsync(int id);

    Task CreateAsync(BlogPostCreateDTO dto);
    Task UpdateAsync(BlogPostUpdateDTO dto);
    Task DeleteAsync(int id);

}
