using NrAcademyBL.DTOs.CourseDTOs;

namespace NrAcademyBL.Services.Abstract
{
    public interface ICourseService
    {
        Task<List<CourseGetDTO>> GetAllAsync();
        Task<CourseGetDTO> GetByIdAsync(int id);
        Task CreateAsync(CourseCreateDTO dto);
        Task UpdateAsync(int id, CourseUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}