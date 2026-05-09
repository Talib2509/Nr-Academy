using NrAcademyBL.DTOs.TestiomonialDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract;

public interface ITestimonialService
{
    Task<List<TestimonialGetDto>> GetAllAsync();
    Task<TestimonialGetDto> GetByIdAsync(int id);
    Task CreateAsync(TestimonialCreateDto dto);
    Task UpdateAsync(TestimonialUpdateDto dto);
    Task DeleteAsync(int id);
}
