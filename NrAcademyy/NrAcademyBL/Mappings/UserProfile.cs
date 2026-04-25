
using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
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

            // TestResult Mapping
            CreateMap<TestResult, TestResultItemDto>().ReverseMap();
            CreateMap<TestResultCreateDto, TestResult>().ReverseMap();
        }
    }
}
