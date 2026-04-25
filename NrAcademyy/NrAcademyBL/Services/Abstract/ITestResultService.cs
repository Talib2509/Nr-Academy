using NrAcademyBL.DTOs.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NrAcademyBL.Services.Abstract
{
    public interface ITestResultService
    {
        Task<IEnumerable<TestResultItemDto>> GetAllAsync();
        Task<TestResultItemDto> GetByIdAsync(int id);
        Task CreateAsync(TestResultCreateDto dto);
        Task DeleteAsync(int id);
    }
}
