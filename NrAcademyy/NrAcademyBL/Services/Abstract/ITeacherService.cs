
using NrAcademyBL.DTOs.TeacherDTOs;

namespace NrAcademyBL.Services.Abstract
{
    public interface ITeacherService
    {
        Task<List<TeacherGetDTO>> GetAllAsync();
        Task<TeacherGetDTO> GetByIdAsync(int id);
        Task CreateAsync(TeacherCreateDTO dto, string rootPath);
        Task UpdateAsync(int id, TeacherUpdateDTO dto, string rootPath);
        Task DeleteAsync(int id, string rootPath);
    }
}