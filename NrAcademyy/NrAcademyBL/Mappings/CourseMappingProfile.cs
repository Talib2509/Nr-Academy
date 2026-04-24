using AutoMapper;
using NrAcademyCORE.Entities;
using NrAcademyBL.DTOs.CourseDTOs;

namespace NrAcademyBL.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {

            CreateMap<CourseCreateDTO, Course>();

            
            CreateMap<CourseUpdateDTO, Course>();

          
            CreateMap<Course, CourseGetDTO>()
                .ForMember(dest => dest.TeacherName,
                    opt => opt.MapFrom(src => src.Teacher.Name));
        }
    }
}