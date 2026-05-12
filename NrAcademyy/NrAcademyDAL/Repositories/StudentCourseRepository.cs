using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities;
using NrAcademyCORE.IRepositories;
using NrAcademyDAL.Context;

namespace NrAcademyDAL.Repositories
{
    public class StudentCourseRepository : GenericRepository<StudentCourse>, IStudentCourseRepository
    {
        private readonly AppDbContext _context;

        public StudentCourseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<StudentCourse>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Course) // Kurs məlumatlarını da gətiririk
                .Where(sc => sc.UserId == studentId)
                .ToListAsync();
        }
    }
}