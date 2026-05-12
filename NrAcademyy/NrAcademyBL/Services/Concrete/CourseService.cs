using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Extensions;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Enums;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        private readonly UserManager<AppUser> _userManager;

        public CourseService(
            ICourseRepository repo,
            IMapper mapper,
            ICacheService cache,
            UserManager<AppUser> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
            _userManager = userManager;
        }

        public async Task<List<CourseGetDTO>> GetAllAsync()
        {
            var key = "courses_all";

            var cached = await _cache.GetAsync<List<CourseGetDTO>>(key);

            if (cached != null)
                return cached;

            var courses = await _repo.GetAllWithTeacherAsync();

            var mapped = _mapper.Map<List<CourseGetDTO>>(courses);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }

        public async Task<CourseGetDTO> GetByIdAsync(int id)
        {
            var key = $"course_{id}";

            var cached = await _cache.GetAsync<CourseGetDTO>(key);

            if (cached != null)
                return cached;

            var course = await _repo.GetByIdWithTeacherAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            var mapped = _mapper.Map<CourseGetDTO>(course);

            await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

            return mapped;
        }

        public async Task CreateAsync(CourseCreateDTO dto, string rootPath)
        {
            var teacher = await _userManager.FindByIdAsync(dto.TeacherId.ToString());

            if (teacher == null)
                throw new Exception("Teacher tapılmadı");

            var isTeacher = await _userManager.IsInRoleAsync(teacher, Roles.Teacher.ToString());

            if (!isTeacher)
                throw new Exception("Seçilən istifadəçi Teacher rolunda deyil");

            var entity = _mapper.Map<Course>(dto);

            if (dto.ImageFile != null)
            {
                if (!dto.ImageFile.IsValidType("image"))
                    throw new Exception("Yalnız şəkil formatı yüklənə bilər.");

                if (!dto.ImageFile.IsValidSize(5000))
                    throw new Exception("Şəkil həcmi 5MB-dan çox ola bilməz.");

                string uploadsFolder = Path.Combine(rootPath, "uploads", "courses");

                entity.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
            }

            await _repo.AddAsync(entity);

            await _cache.RemoveAsync("courses_all");
        }

        public async Task UpdateAsync(int id, CourseUpdateDTO dto, string rootPath)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            if (dto.TeacherId > 0)
            {
                var teacher = await _userManager.FindByIdAsync(dto.TeacherId.ToString());

                if (teacher == null)
                    throw new Exception("Teacher tapılmadı");

                var isTeacher = await _userManager.IsInRoleAsync(teacher, Roles.Teacher.ToString());

                if (!isTeacher)
                    throw new Exception("Seçilən istifadəçi Teacher rolunda deyil");
            }

            if (dto.ImageFile != null)
            {
                if (!dto.ImageFile.IsValidType("image"))
                    throw new Exception("Yalnız şəkil formatı yüklənə bilər.");

                if (!dto.ImageFile.IsValidSize(5000))
                    throw new Exception("Şəkil həcmi 5MB-dan çox ola bilməz.");

                if (!string.IsNullOrEmpty(course.ImageUrl))
                {
                    FileExtensions.DeleteFile(
                        Path.GetFileName(course.ImageUrl),
                        rootPath,
                        "uploads",
                        "courses"
                    );
                }

                string uploadsFolder = Path.Combine(rootPath, "uploads", "courses");

                course.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
            }

            _mapper.Map(dto, course);

            await _repo.UpdateAsync(course);

            await _cache.RemoveAsync("courses_all");
            await _cache.RemoveAsync($"course_{id}");
        }

        public async Task DeleteAsync(int id, string rootPath)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            if (!string.IsNullOrEmpty(course.ImageUrl))
            {
                FileExtensions.DeleteFile(
                    Path.GetFileName(course.ImageUrl),
                    rootPath,
                    "uploads",
                    "courses"
                );
            }

            await _repo.DeleteAsync(course);

            await _cache.RemoveAsync("courses_all");
            await _cache.RemoveAsync($"course_{id}");
        }
    }
}