
using AutoMapper;
using NrAcademyBL.DTOs.BlogCategoryDTO;
using NrAcademyCORE.Entities;

public class BlogCategoryProfile : Profile
{
    public BlogCategoryProfile()
    {
        CreateMap<BlogCategory, BlogCategoryGetDTO>().ReverseMap();
        CreateMap<BlogCategoryCreateDTO, BlogCategory>();
        CreateMap<BlogCategoryUpdateDTO, BlogCategory>();
    }
}