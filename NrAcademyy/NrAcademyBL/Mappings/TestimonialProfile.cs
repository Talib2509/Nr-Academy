using AutoMapper;
using NrAcademyBL.DTOs.TestiomonialDTO;
using NrAcademyCORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Mappings;

public class TestimonialProfile:Profile
{
    public TestimonialProfile()
    {
        CreateMap<Testimonial, TestimonialGetDto>();
        CreateMap<TestimonialCreateDto, Testimonial>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<TestimonialUpdateDto, Testimonial>();
    }
}
