using NrAcademyBL.DTOs.TestResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NrAcademyBL.Services.Abstract
{
    public interface ITestResultService
    {
        Task<List<TestResultItemDto>> GetAllAsync();
        Task<TestResultItemDto> GetByIdAsync(int id);
        Task CreateAsync(TestResultCreateDto dto);
        Task DeleteAsync(int id);
    }
}
