using AutoMapper;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;

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
        public async Task CreateAsync(TeacherCreateDTO dto)
        {
            var teacher = _mapper.Map<Teacher>(dto);
            await _repo.AddAsync(teacher);

            await _cache.RemoveAsync("teachers_all");
        }
        public async Task UpdateAsync(int id, TeacherUpdateDTO dto)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            _mapper.Map(dto, teacher);
            _repo.Update(teacher);

            await _cache.RemoveAsync("teachers_all");
            await _cache.RemoveAsync($"teacher_{id}");
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            _repo.Delete(teacher);

            await _cache.RemoveAsync("teachers_all");
            await _cache.RemoveAsync($"teacher_{id}");
        }
    }
}
