
using AutoMapper;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyCORE.Entities;

public class CertificateProfile : Profile
{
    public CertificateProfile()
    {
        CreateMap<Certificate, CertificateGetDTO>();
        CreateMap<CertificateCreateDTO, Certificate>();
        CreateMap<CertificateUpdateDTO, Certificate>();
    }
}