using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class CertificateService: ICertificateService
    {
        private readonly ICertificateRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public CertificateService(ICertificateRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task CreateAsync(CertificateCreateDTO dto)
        {
            var certificate = _mapper.Map<Certificate>(dto);
            await _repo.AddAsync(certificate);
          

            await _cache.RemoveAsync("certificates_all");
        }

        public async Task DeleteAsync(int id)
        {
            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null)
                throw new Exception("Silinecek sertifikat tapılmadı");

            _repo.Delete(certificate);
          

            await _cache.RemoveAsync("certificates_all");
            await _cache.RemoveAsync($"certificate_{id}");
        }

        public async Task<List<CertificateGetDTO>> GetAllAsync()
        {
            var key = "certificates_all";

            var cached = await _cache.GetAsync<List<CertificateGetDTO>>(key);
            if (cached != null)
                return cached;

            var certificates = await _repo.GetAll().ToListAsync();
            var mapped = _mapper.Map<List<CertificateGetDTO>>(certificates);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(20));

            return mapped;
        }

        public async Task<CertificateGetDTO> GetByIdAsync(int id)
        {
            var key = $"certificate_{id}";

            var cached = await _cache.GetAsync<CertificateGetDTO>(key);
            if (cached != null)
                return cached;

            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null)
                throw new Exception("Sertifikat tapilmadi");

            var mapped = _mapper.Map<CertificateGetDTO>(certificate);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(20));

            return mapped;
        }

        public async Task UpdateAsync(CertificateUpdateDTO dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new Exception("Yenilenecek sertifikat tapilmadi");

            _mapper.Map(dto, existing);
            _repo.Update(existing);
          

            await _cache.RemoveAsync("certificates_all");
            await _cache.RemoveAsync($"certificate_{dto.Id}");
        }
    }
}
