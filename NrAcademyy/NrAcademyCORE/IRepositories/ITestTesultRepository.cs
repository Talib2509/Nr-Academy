using NrAcademyCORE.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NrAcademyCORE.Repositories
{
    public interface ITestResultRepository : IGenericRepository<TestResult>
    {
        Task<TestResult> GetWinnerForTestAsync(int testId);
        
        Task<List<TestResult>> GetResultsByTestIdWithUserAsync(int testId);
    }
}