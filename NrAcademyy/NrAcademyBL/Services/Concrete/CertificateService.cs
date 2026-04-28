using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.CertificateDTO;
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
    public class CertificateService(ICertificateRepository _repo, IMapper _mapper) : ICertificateService
    {
        public async Task CreateAsync(CertificateCreateDTO dto)
        {
            var certificate = _mapper.Map<Certificate>(dto);
            await _repo.AddAsync(certificate);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null) throw new Exception("Silinecek sertifikat tapılmadı");

            _repo.Delete(certificate);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<CertificateGetDTO>> GetAllAsync()
        {
            var certificates = await _repo.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<CertificateGetDTO>>(certificates);
        }

        public async Task<CertificateGetDTO> GetByIdAsync(int id)
        {
            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null) throw new Exception("Sertifikat tapilmadi");
            return _mapper.Map<CertificateGetDTO>(certificate);
        }

        public async Task UpdateAsync(CertificateUpdateDTO dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) throw new Exception("Yenilenecek sertifikat tapilmadi");

            _mapper.Map(dto, existing);
            _repo.Update(existing);
            await _repo.SaveAsync();
        }
    }
}
