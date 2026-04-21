using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using NrAcademyDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories
{
    
}
public class TestRepository : GenericRepository<Test>, ITestRepository
{
    public TestRepository(AppDbContext _context) : base(_context)
    {

    }
}