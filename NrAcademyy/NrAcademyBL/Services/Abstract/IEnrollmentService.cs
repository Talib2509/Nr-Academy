using NrAcademyBL.DTOs.EnrollmentDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentGetDTO>> GetAllAsync();
        Task<EnrollmentGetDTO> GetByIdAsync(int id);
        Task CreateAsync(EnrollmentCreateDTO dto);
        Task UpdateAsync(EnrollmentUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
