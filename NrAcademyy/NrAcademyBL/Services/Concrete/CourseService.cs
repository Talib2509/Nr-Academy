using AutoMapper;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;

namespace NrAcademyBL.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        public CourseService(ICourseRepository repo, IMapper mapper, ICacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
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


        public async Task CreateAsync(CourseCreateDTO dto)
        {
            var entity = _mapper.Map<Course>(dto);
            await _repo.AddAsync(entity);

            await _cache.RemoveAsync("courses_all");
        }

        public async Task UpdateAsync(int id, CourseUpdateDTO dto)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            _mapper.Map(dto, course);
            _repo.Update(course);

            await _cache.RemoveAsync("courses_all");
            await _cache.RemoveAsync($"course_{id}");
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            _repo.Delete(course);

            await _cache.RemoveAsync("courses_all");
            await _cache.RemoveAsync($"course_{id}");
        }
    }
}