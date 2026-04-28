using AutoMapper;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyBL.Extensions;

namespace NrAcademyBL.Services.Concrete
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repo;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task CreateAsync(TeacherCreateDTO dto, string rootPath)
        {
            var teacher = _mapper.Map<Teacher>(dto);

            if (dto.ImageFile != null)
            {
                teacher.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "teachers");
            }

            await _repo.AddAsync(teacher);
            await _repo.SaveAsync();
        }

        public async Task UpdateAsync(int id, TeacherUpdateDTO dto, string rootPath)
        {
            var teacher = await _repo.GetByIdAsync(id);
            if (teacher == null) throw new Exception("Teacher not found");

            if (dto.ImageFile != null)
            {
                // Köhnə şəkli serverdən silirik
                if (!string.IsNullOrEmpty(teacher.ImageUrl))
                {
                    FileExtensions.DeleteFile(teacher.ImageUrl, rootPath, "uploads", "teachers");
                }
                teacher.ImageUrl = await dto.ImageFile.UploadAsync(rootPath, "uploads", "teachers");
            }

            _mapper.Map(dto, teacher);
            _repo.Update(teacher);
            await _repo.SaveAsync();
        }

        public async Task DeleteAsync(int id, string rootPath)
        {
            var teacher = await _repo.GetByIdAsync(id);
            if (teacher == null) throw new Exception("Teacher not found");

            if (!string.IsNullOrEmpty(teacher.ImageUrl))
            {
                FileExtensions.DeleteFile(teacher.ImageUrl, rootPath, "uploads", "teachers");
            }

            _repo.Delete(teacher);
            await _repo.SaveAsync();
        }

        public async Task<List<TeacherGetDTO>> GetAllAsync()
        {
            var teachers = await _repo.GetAllAsync();
            return _mapper.Map<List<TeacherGetDTO>>(teachers);
        }

        public async Task<TeacherGetDTO> GetByIdAsync(int id)
        {
            var teacher = await _repo.GetByIdAsync(id);
            if (teacher == null) throw new Exception("Teacher not found");
            return _mapper.Map<TeacherGetDTO>(teacher);
        }
    }
}