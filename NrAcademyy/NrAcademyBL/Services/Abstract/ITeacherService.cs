using NrAcademyBL.DTOs.TeacherDTOs;

namespace NrAcademyBL.Services.Abstract
{
    public interface ITeacherService
    {
        Task<List<TeacherGetDTO>> GetAllAsync();
        Task<TeacherGetDTO> GetByIdAsync(int id);
        Task CreateAsync(TeacherCreateDTO dto);
        Task UpdateAsync(int id, TeacherUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}