using NrAcademyBL.DTOs.TestResultDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface ITestResultService
    {
        Task<List<TestResultItemDto>> GetAllAsync();
        Task<TestResultItemDto> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task DetermineWinnerForTestAsync(int testId);
        Task<List<TestResultItemDto>> GetUserResultsAsync(int userId);
        Task<TestResultItemDto> SubmitTestAsync(int userId, TestSubmitDto dto);

        
        Task<List<TestResultItemDto>> GetResultsByTestIdAsync(int testId);
    }
}