using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;

namespace NrAcademyBL.Services.Abstract
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionItemDto>> GetAllAsync();
        Task<QuestionItemDto> GetByIdAsync(int id);
        Task CreateAsync(QuestionCreateDto dto);
        Task UpdateAsync(QuestionUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
