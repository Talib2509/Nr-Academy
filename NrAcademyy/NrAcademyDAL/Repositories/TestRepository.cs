using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;

namespace NrAcademyDAL.Repositories
{
    public class TestRepository : GenericRepository<Test>, ITestRepository
    {
        public TestRepository(AppDbContext context) : base(context)
        {
        }
    }
}