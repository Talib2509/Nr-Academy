using AutoMapper;
using NrAcademyBL.DTOs.Course;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity; // AppUser üçün lazımdır

namespace NrAcademyBL.Mappings
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile()
        {
            // 1. Entity-dən DTO-ya (Müəllim cədvəli istifadə edildikdə)
            CreateMap<Teacher, TeacherGetDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            // 2. AppUser-dən DTO-ya (UserManager.GetUsersInRoleAsync istifadə edildikdə)
            // Bu hissə sizin aldığınız "List<AppUser> -> List<TeacherGetDTO>" xətasını həll edir
            CreateMap<AppUser, TeacherGetDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio));

            // 3. Digər mapping-lər
            CreateMap<TeacherCreateDTO, Teacher>();
            CreateMap<TeacherUpdateDTO, Teacher>();
            CreateMap<Teacher, TeacherInCourseDTO>();

            // Əgər Course içində müəllim məlumatları AppUser-dən gəlirsə bunu da əlavə edin:
            CreateMap<AppUser, TeacherInCourseDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
        }
    }
}