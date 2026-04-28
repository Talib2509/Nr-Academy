using AutoMapper;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyBL.Extensions; 

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

        public async Task CreateAsync(CourseCreateDTO dto, string rootPath)
        {
            var entity = _mapper.Map<Course>(dto);

            if (dto.ImageFile != null)
            {
                
                entity.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "courses");
            }

            await _repo.AddAsync(entity);
            await _repo.SaveAsync(); 
        }

        public async Task UpdateAsync(int id, CourseUpdateDTO dto, string rootPath)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) throw new Exception("Course not found");

            if (dto.ImageFile != null)
            {
                // Köhnə şəkli silirik
                if (!string.IsNullOrEmpty(course.ImageUrl))
                {
                    FileExtensions.DeleteFile(course.ImageUrl, rootPath, "uploads", "courses");
                }
                // Yenisini yükləyirik
                course.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "courses");
            }

            _mapper.Map(dto, course);
            _repo.Update(course);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id, string rootPath)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) throw new Exception("Course not found");

            // Kurs silinəndə şəkli də silirik
            if (!string.IsNullOrEmpty(course.ImageUrl))
            {
                FileExtensions.DeleteFile(course.ImageUrl, rootPath, "uploads", "courses");
            }

            _repo.Delete(course);
            await _repo.SaveAsync();
        }

        public async Task<List<CourseGetDTO>> GetAllAsync()
        {
            var courses = await _repo.GetAllWithTeacherAsync();
            return _mapper.Map<List<CourseGetDTO>>(courses);
        }

        public async Task<CourseGetDTO> GetByIdAsync(int id)
        {
            var course = await _repo.GetByIdWithTeacherAsync(id);
            if (course == null) throw new Exception("Course not found");
            return _mapper.Map<CourseGetDTO>(course);
        }
    }
}