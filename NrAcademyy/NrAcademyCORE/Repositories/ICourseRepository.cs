using NrAcademyCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<List<Course>> GetAllWithTeacherAsync();
        Task<Course> GetByIdWithTeacherAsync(int id);
    }
}
