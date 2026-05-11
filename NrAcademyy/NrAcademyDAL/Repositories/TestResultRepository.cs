using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories
{
    public class TestResultRepository : GenericRepository<TestResult>, ITestResultRepository
    {
        public TestResultRepository(AppDbContext context) : base(context) { }

        public async Task<TestResult> GetWinnerForTestAsync(int testId)
        {
            return await _context.TestResults
                .Where(tr => tr.TestId == testId)
                .OrderByDescending(tr => tr.Score)
                .ThenBy(tr => EF.Functions.DateDiffMillisecond(tr.StartedAt, tr.CompletedAt))
                .FirstOrDefaultAsync();
        }

        public async Task<List<TestResult>> GetResultsByTestIdWithUserAsync(int testId)
        {
            return await _context.TestResults
                .Include(tr => tr.User) // Tələbə adını görmək üçün
                .Where(tr => tr.TestId == testId)
                .OrderByDescending(tr => tr.Score)
                .ToListAsync();
        }
    }
}