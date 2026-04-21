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
    public class JobApplicationRepository : GenericRepository<Enrollment>, IJobApplicationRepository
    {
        public JobApplicationRepository(AppDbContext _context) : base(_context)
        {

        }
    }
}
