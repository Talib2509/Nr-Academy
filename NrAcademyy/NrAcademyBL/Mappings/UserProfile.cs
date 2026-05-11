using AutoMapper;
using NrAcademyBL.DTOs.AnswerDTO;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.DTOs.QuestionDTO;
using NrAcademyBL.DTOs.TestDTO;
using NrAcademyBL.DTOs.TestResultDTO;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using static NrAcademyBL.DTOs.TestDTO.TestDTO;

namespace NrAcademyBL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, AppUser>();

            // Answer Mappings
            CreateMap<AnswerCreateDto, Answer>().ForMember(d => d.AnswerText, o => o.MapFrom(s => s.Text));
            CreateMap<AnswerUpdateDto, Answer>().ForMember(d => d.AnswerText, o => o.MapFrom(s => s.Text));
            CreateMap<Answer, AnswerItemDto>().ForMember(d => d.Text, o => o.MapFrom(s => s.AnswerText));

            // Question Mappings
            CreateMap<QuestionCreateDto, Question>()
                .ForMember(d => d.QuestionText, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.QuestionType, o => o.MapFrom(s => s.QuestionType));
            CreateMap<QuestionUpdateDto, Question>()
                .ForMember(d => d.QuestionText, o => o.MapFrom(s => s.Text));
            CreateMap<Question, QuestionItemDto>().ForMember(d => d.Text, o => o.MapFrom(s => s.QuestionText));

            // Test Mappings
            CreateMap<Test, TestCreateDto>().ReverseMap();
            CreateMap<Test, TestUpdateDto>().ReverseMap();
            CreateMap<Test, TestItemDto>().ReverseMap();

            // TestResult Mappings 
            
            CreateMap<TestResult, TestResultItemDto>()
                .ForMember(d => d.AppUserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => s.User.FirstName + " " + s.User.LastName)) 
                .ReverseMap();

            CreateMap<TestResultCreateDto, TestResult>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.AppUserId));

            CreateMap<TestResultUpdateDto, TestResult>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.AppUserId))
                .ReverseMap();

            // Certificate Mappings
            CreateMap<Certificate, CertificateGetDTO>().ReverseMap();
            CreateMap<CertificateCreateDTO, Certificate>().ReverseMap();
            CreateMap<CertificateUpdateDTO, Certificate>().ReverseMap();
        }
    }
}