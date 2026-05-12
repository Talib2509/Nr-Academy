using AutoMapper;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyCORE.Entities;

namespace NrAcademyBL.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            CreateMap<CourseCreateDTO, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore());

            CreateMap<CourseUpdateDTO, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore());

            CreateMap<Course, CourseGetDTO>();


        }
    }
}