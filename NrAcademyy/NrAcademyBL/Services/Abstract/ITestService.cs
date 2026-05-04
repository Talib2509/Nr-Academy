using NrAcademyBL.DTOs.TestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NrAcademyBL.DTOs.TestDTO.TestDTO;

namespace NrAcademyBL.Services.Abstract
{
    public interface ITestService
    {
        Task<List<TestItemDto>> GetAllAsync();
        Task<TestItemDto> GetByIdAsync(int id);
        Task CreateAsync(TestCreateDto dto);
        Task UpdateAsync(TestUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
