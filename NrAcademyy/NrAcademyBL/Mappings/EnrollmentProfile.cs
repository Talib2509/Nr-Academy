
using AutoMapper;
using NrAcademyBL.DTOs.EnrollmentDTO;
using NrAcademyCORE.Entities;

public class EnrollmentProfile : Profile
{
    public EnrollmentProfile()
    {
      
        CreateMap<Enrollment, EnrollmentGetDTO>();

       
        CreateMap<EnrollmentCreateDTO, Enrollment>();


        CreateMap<EnrollmentUpdateDTO, Enrollment>();
    }
}