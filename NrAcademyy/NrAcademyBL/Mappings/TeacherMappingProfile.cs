using AutoMapper;

using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyCORE.Entities;

namespace NrAcademyBL.Mappings
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            CreateMap<Teacher, TeacherGetDTO>();
            CreateMap<TeacherCreateDTO, Teacher>();
            CreateMap<TeacherUpdateDTO, Teacher>();
        }
    }
}