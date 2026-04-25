using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext _context) : base(_context)
        {
        }
        public async Task<List<Course>> GetAllWithTeacherAsync()
        {
            return await _context.Courses
                .Include(x => x.Teacher)
                .ToListAsync();
        }

        public async Task<Course> GetByIdWithTeacherAsync(int id)
        {
            return await _context.Courses
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

