using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public CertificateService(ICertificateRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;

            // QuestPDF lisenziyası
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task CreateAsync(CertificateCreateDTO dto)
        {
            
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "certificates");

            
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath);
            }

            // 2. Fayl üçün unikal ad yaradırıq (məs: user_5_12345678.pdf)
            var fileName = $"user_{dto.UserId}_{Guid.NewGuid().ToString().Substring(0, 8)}.pdf";
            var fullPath = Path.Combine(wwwrootPath, fileName);

            
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape()); 
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(18).FontFamily(Fonts.Arial));

                    page.Content().Column(col =>
                    {
                        col.Spacing(20);

                        col.Item().AlignCenter().Text("MÜVƏFFƏQİYYƏT SERTİFİKATİ")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Darken3);

                        col.Item().AlignCenter().Text("Bu sertifikat təqdim olunur:")
                            .FontSize(20).FontColor(Colors.Grey.Medium);

                        col.Item().AlignCenter().Text(string.IsNullOrEmpty(dto.UserFullName) ? $"Tələbə ID: {dto.UserId}" : dto.UserFullName)
                            .SemiBold().FontSize(30).FontColor(Colors.Black);

                        col.Item().AlignCenter().Text($"Aşağıdakı imtahandan yüksək nəticə göstərdiyinə görə:")
                            .FontSize(20);

                        col.Item().AlignCenter().Text(dto.TestTitle)
                            .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

                        col.Item().AlignCenter().Text($"Topladığı Bal: {dto.Score}%")
                            .SemiBold().FontSize(22).FontColor(Colors.Green.Darken2);

                        col.Item().AlignCenter().Text($"Sertifikat Növü: {dto.CertificateType}")
                            .FontSize(18).FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(30).AlignCenter().Text($"Verilmə Tarixi: {DateTime.Now:dd.MM.yyyy}")
                            .FontSize(16);
                    });
                });
            })
            .GeneratePdf(fullPath);

            
            var certificate = _mapper.Map<Certificate>(dto);

            
            certificate.CertificateUrl = $"/certificates/{fileName}";

            await _repo.AddAsync(certificate);
            await _cache.RemoveAsync("certificates_all");
        }

        public async Task DeleteAsync(int id)
        {
            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null)
                throw new Exception("Silinəcək sertifikat tapılmadı");

            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", certificate.CertificateUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await _repo.DeleteAsync(certificate);
            await _cache.RemoveAsync("certificates_all");
            await _cache.RemoveAsync($"certificate_{id}");
        }

        public async Task<List<CertificateGetDTO>> GetAllAsync()
        {
            var key = "certificates_all";
            var cached = await _cache.GetAsync<List<CertificateGetDTO>>(key);
            if (cached != null) return cached;

            var certificates = await _repo.GetAll().ToListAsync();
            var mapped = _mapper.Map<List<CertificateGetDTO>>(certificates);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(20));
            return mapped;
        }

        public async Task<CertificateGetDTO> GetByIdAsync(int id)
        {
            var key = $"certificate_{id}";
            var cached = await _cache.GetAsync<CertificateGetDTO>(key);
            if (cached != null) return cached;

            var certificate = await _repo.GetByIdAsync(id);
            if (certificate == null) throw new Exception("Sertifikat tapılmadı");

            var mapped = _mapper.Map<CertificateGetDTO>(certificate);
            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(20));
            return mapped;
        }

        public async Task UpdateAsync(CertificateUpdateDTO dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) throw new Exception("Yenilənəcək sertifikat tapılmadı");

            _mapper.Map(dto, existing);
            await _repo.UpdateAsync(existing);

            await _cache.RemoveAsync("certificates_all");
            await _cache.RemoveAsync($"certificate_{dto.Id}");
        }
    }
}