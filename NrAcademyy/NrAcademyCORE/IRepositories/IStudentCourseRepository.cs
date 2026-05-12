using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;

namespace NrAcademyCORE.IRepositories
{
    public interface IStudentCourseRepository : IGenericRepository<StudentCourse>
    {
      
        Task<List<StudentCourse>> GetCoursesByStudentIdAsync(int studentId);
    }
}