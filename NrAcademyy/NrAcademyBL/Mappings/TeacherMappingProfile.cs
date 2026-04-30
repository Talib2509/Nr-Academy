using AutoMapper;
using NrAcademyBL.DTOs.Course;
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
            // Bu map çatışmadığı üçün xəta verir
            CreateMap<Teacher, TeacherInCourseDTO>();
        }
    }
}