// NrAcademyBL/Services/Concrete/TeacherService.cs
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Extensions;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public TeacherService(ITeacherRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<TeacherGetDTO>> GetAllAsync()
        {
            var key = "teachers_all";

            var cached = await _cache.GetAsync<List<TeacherGetDTO>>(key);
            if (cached != null)
                return cached;

            var teachers = await _repo.GetAllAsync();
            var mapped = _mapper.Map<List<TeacherGetDTO>>(teachers);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

            return mapped;
        }

        public async Task<TeacherGetDTO> GetByIdAsync(int id)
        {
            var key = $"teacher_{id}";

            var cached = await _cache.GetAsync<TeacherGetDTO>(key);
            if (cached != null)
                return cached;

            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            var mapped = _mapper.Map<TeacherGetDTO>(teacher);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

            return mapped;
        }

        public async Task CreateAsync(TeacherCreateDTO dto, string rootPath)
        {
            var teacher = _mapper.Map<Teacher>(dto);

            if (dto.ImageFile != null)
            {
                // FileExtensions vasitəsilə faylı yükləyirik
                string uploadsFolder = Path.Combine(rootPath, "uploads", "teachers");
                teacher.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
            }

            await _repo.AddAsync(teacher);
            await _cache.RemoveAsync("teachers_all");
        }

        public async Task UpdateAsync(int id, TeacherUpdateDTO dto, string rootPath)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            if (dto.ImageFile != null)
            {
                // Köhnə şəkili sil
                if (!string.IsNullOrEmpty(teacher.ImageUrl))
                {
                    FileExtensions.DeleteFile(Path.GetFileName(teacher.ImageUrl), rootPath, "uploads", "teachers");
                }

                // Yeni şəkili yüklə
                string uploadsFolder = Path.Combine(rootPath, "uploads", "teachers");
                teacher.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
            }

            _mapper.Map(dto, teacher);
            _repo.Update(teacher);

            await _cache.RemoveAsync("teachers_all");
            await _cache.RemoveAsync($"teacher_{id}");
        }

        public async Task DeleteAsync(int id, string rootPath)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            // Şəkili sil
            if (!string.IsNullOrEmpty(teacher.ImageUrl))
            {
                FileExtensions.DeleteFile(Path.GetFileName(teacher.ImageUrl), rootPath, "uploads", "teachers");
            }

            _repo.Delete(teacher);
            await _cache.RemoveAsync("teachers_all");
            await _cache.RemoveAsync($"teacher_{id}");
        }
    }
}