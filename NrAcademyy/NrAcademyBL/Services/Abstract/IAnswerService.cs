using NrAcademyBL.DTOs.AnswerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NrAcademyBL.Services.Abstract
{
    public interface IAnswerService
    {
        Task<List<AnswerItemDto>> GetAllAsync();
        Task<AnswerItemDto> GetByIdAsync(int id);
        Task CreateAsync(AnswerCreateDto dto);
        Task UpdateAsync(AnswerUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
