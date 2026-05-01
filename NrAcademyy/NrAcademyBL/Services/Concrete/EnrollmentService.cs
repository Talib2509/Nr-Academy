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
using NrAcademyBL.Extensions.Caching;


namespace NrAcademyBL.Services.Concrete
{
    public class EnrollmentService: IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        public EnrollmentService(IEnrollmentRepository repo, IMapper mapper, ICacheService cacheService)
        {
            _repo = repo;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task CreateAsync(EnrollmentCreateDTO dto)
        {
            var enrollment = _mapper.Map<Enrollment>(dto);
            await _repo.AddAsync(enrollment);

            await _cacheService.RemoveAsync("enrollments_all");
        }

        public async Task DeleteAsync(int id)
        {
            var enrollment = await _repo.GetByIdAsync(id);
            if (enrollment == null)
                throw new Exception("Silinecek qeydiyyat tapılmadı");

            _repo.Delete(enrollment);
            await _repo.SaveAsync();

            await _cacheService.RemoveAsync("enrollments_all");
            await _cacheService.RemoveAsync($"enrollment_{id}");
        }

        public async Task<List<EnrollmentGetDTO>> GetAllAsync()
        {
            var key = "enrollments_all";

            var cached = await _cacheService.GetAsync<List<EnrollmentGetDTO>>(key);
            if (cached != null)
                return cached;

            var enrollments = await _repo.GetAll().ToListAsync();
            var mapped = _mapper.Map<List<EnrollmentGetDTO>>(enrollments);

            await _cacheService.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }

        public async Task<EnrollmentGetDTO> GetByIdAsync(int id)
        {
            var key = $"enrollment_{id}";

            var cached = await _cacheService.GetAsync<EnrollmentGetDTO>(key);
            if (cached != null)
                return cached;

            var enrollment = await _repo.GetByIdAsync(id);
            if (enrollment == null)
                throw new Exception("Qeydiyyat tapılmadı");

            var mapped = _mapper.Map<EnrollmentGetDTO>(enrollment);

            await _cacheService.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }

        public async Task UpdateAsync(EnrollmentUpdateDTO dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new Exception("Yenilənecek qeydiyyat tapılmadı");

            _mapper.Map(dto, existing);
            _repo.Update(existing);
            await _repo.SaveAsync();

            await _cacheService.RemoveAsync("enrollments_all");
            await _cacheService.RemoveAsync($"enrollment_{dto.Id}");
        }
    }
}
