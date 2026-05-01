
using NrAcademyBL.DTOs.CourseDTOs;

namespace NrAcademyBL.Services.Abstract
{
    public interface ICourseService
    {
        Task<List<CourseGetDTO>> GetAllAsync();
        Task<CourseGetDTO> GetByIdAsync(int id);
        Task CreateAsync(CourseCreateDTO dto, string rootPath);
        Task UpdateAsync(int id, CourseUpdateDTO dto, string rootPath);
        Task DeleteAsync(int id, string rootPath);
    }
}