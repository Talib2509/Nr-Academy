using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;

namespace NrAcademyBL.Services.Abstract
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerItemDto>> GetAllAsync();
        Task<AnswerItemDto> GetByIdAsync(int id);
        Task CreateAsync(AnswerCreateDto dto);
        Task UpdateAsync(AnswerUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
