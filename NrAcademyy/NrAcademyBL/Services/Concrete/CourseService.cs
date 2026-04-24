using AutoMapper;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;

namespace NrAcademyBL.Services.Concrete
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CourseGetDTO>> GetAllAsync()
        {
            var courses = await _repo.GetAllWithTeacherAsync();
            return _mapper.Map<List<CourseGetDTO>>(courses);
        }

        public async Task<CourseGetDTO> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdWithTeacherAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            return _mapper.Map<CourseGetDTO>(course);
        }

        public async Task CreateAsync(CourseCreateDTO dto)
        {
            var entity = _mapper.Map<Course>(dto);
            await _repo.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, CourseUpdateDTO dto)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            _mapper.Map(dto, course);
            _repo.Update(course);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _repo.GetByIdAsync(id);

            if (course == null)
                throw new Exception("Course not found");

            _repo.Delete(course);
        }
    }
}