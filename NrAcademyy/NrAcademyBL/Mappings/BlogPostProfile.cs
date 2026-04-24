
using AutoMapper;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyCORE.Entities;

public class BlogPostProfile : Profile
{
    public BlogPostProfile()
    {
       
        CreateMap<BlogPost, BlogPostGetDTO>()
          .ForMember(dest => dest.CategoryName, options => options.MapFrom(x => x.Category != null ? x.Category.Name : "Kateqoriya yoxdur"));


       
        CreateMap<BlogPostCreateDTO, BlogPost>();

        CreateMap<BlogPostUpdateDTO, BlogPost>();
    }
}
