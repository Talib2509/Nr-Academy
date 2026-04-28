using AutoMapper;
using NrAcademyBL.DTOs.EnrollmentDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace NrAcademyBL.Services.Concrete
{
    public class EnrollmentService(IEnrollmentRepository _repo, IMapper _mapper) : IEnrollmentService
    {
      
        public async Task CreateAsync(EnrollmentCreateDTO dto)
        {

            var enrollment = _mapper.Map<Enrollment>(dto);
            await _repo.AddAsync(enrollment);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var enrollment = await _repo.GetByIdAsync(id);
            if (enrollment == null) throw new Exception("Silinecek qeydiyyat tapılmadı ");

            _repo.Delete(enrollment);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<EnrollmentGetDTO>> GetAllAsync()
        {
            var enrollments = await _repo.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<EnrollmentGetDTO>>(enrollments);
        }

        public async Task<EnrollmentGetDTO> GetByIdAsync(int id)
        {
            var enrollment = await _repo.GetByIdAsync(id);
            if (enrollment == null) throw new Exception("Qeydiyyat tapılmadı ");

            return _mapper.Map<EnrollmentGetDTO>(enrollment);
        }

        public async Task UpdateAsync(EnrollmentUpdateDTO dto)
        {

            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) throw new Exception("Yenilənecek qeydiyyat tapılmadı ");

            _mapper.Map(dto, existing);
            _repo.Update(existing);
            await _repo.SaveAsync();
        }
    }
}
