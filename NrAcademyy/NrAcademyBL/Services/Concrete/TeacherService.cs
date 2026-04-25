using AutoMapper;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;

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

        public async Task<List<TeacherGetDTO>> GetAllAsync()
        {
            var teachers = await _repo.GetAllAsync();
            return _mapper.Map<List<TeacherGetDTO>>(teachers);
        }

        public async Task<TeacherGetDTO> GetByIdAsync(int id)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            return _mapper.Map<TeacherGetDTO>(teacher);
        }

        public async Task CreateAsync(TeacherCreateDTO dto)
        {
            var teacher = _mapper.Map<Teacher>(dto);
            await _repo.AddAsync(teacher);
        }

        public async Task UpdateAsync(int id, TeacherUpdateDTO dto)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            _mapper.Map(dto, teacher);
            _repo.Update(teacher);
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = await _repo.GetByIdAsync(id);

            if (teacher == null)
                throw new Exception("Teacher not found");

            _repo.Delete(teacher);
        }
    }
}
