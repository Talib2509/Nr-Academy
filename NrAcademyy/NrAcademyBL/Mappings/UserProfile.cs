using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;
using static NrAcademyBL.DTOs.AuthDTO.TestDTO;


namespace NrAcademyBL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDTO, AppUser>();

            // Answer
            CreateMap<AnswerCreateDto, Answer>().ForMember(d => d.AnswerText, o => o.MapFrom(s => s.Text));
            CreateMap<AnswerUpdateDto, Answer>().ForMember(d => d.AnswerText, o => o.MapFrom(s => s.Text));
            CreateMap<Answer, AnswerItemDto>().ForMember(d => d.Text, o => o.MapFrom(s => s.AnswerText));

            // Question
            CreateMap<QuestionCreateDto, Question>()
                .ForMember(d => d.QuestionText, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.QuestionType, o => o.MapFrom(s => s.QuestionType));
            CreateMap<QuestionUpdateDto, Question>()
                .ForMember(d => d.QuestionText, o => o.MapFrom(s => s.Text));
            CreateMap<Question, QuestionItemDto>().ForMember(d => d.Text, o => o.MapFrom(s => s.QuestionText));

            // Test
            CreateMap<Test, TestCreateDto>().ReverseMap();
            CreateMap<Test, TestUpdateDto>().ReverseMap();
            CreateMap<Test, TestItemDto>().ReverseMap();

            // Course Mappings
            CreateMap<CourseCreateDTO, Course>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<CourseUpdateDTO, Course>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Course, CourseGetDTO>().ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));

            // Teacher Mappings
            CreateMap<TeacherCreateDTO, Teacher>().ForMember(d => d.ImageUrl, opt => opt.Ignore());
            CreateMap<TeacherUpdateDTO, Teacher>().ForMember(d => d.ImageUrl, opt => opt.Ignore());
            CreateMap<Teacher, TeacherGetDTO>();

            // BlogPost Mappings
            CreateMap<BlogPostCreateDTO, BlogPost>().ForMember(d => d.ImageUrl, opt => opt.Ignore());
            CreateMap<BlogPostUpdateDTO, BlogPost>().ForMember(d => d.ImageUrl, opt => opt.Ignore());
            CreateMap<BlogPost, BlogPostGetDTO>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(x => x.Category != null ? x.Category.Name : "Kateqoriya yoxdur"));
         
            // TestResult Mapping
            CreateMap<TestResult, TestResultItemDto>().ReverseMap();
            CreateMap<TestResultCreateDto, TestResult>().ReverseMap();
        }
    }
}
