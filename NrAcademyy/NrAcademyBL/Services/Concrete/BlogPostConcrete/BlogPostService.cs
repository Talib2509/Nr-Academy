using AutoMapper;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Services.Abstract.BlogPostAbstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace NrAcademyBL.Services.Concrete.BlogPostConcrete;

public class BlogPostService(IBlogPostRepository _repository,IMapper _mapper) : IBlogPostService
{
    public async Task CreateAsync(BlogPostCreateDTO dto)
    {
        var newBlogPost = _mapper.Map<BlogPost>(dto);

        await _repository.AddAsync(newBlogPost);
        await _repository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
       var blogPosts=await _repository.GetByIdAsync(id);

        if(blogPosts == null)
        {
            throw new Exception("Silmek istediyiniz Post Tapilmadi");
        }

        _repository.Delete(blogPosts);
        await _repository.SaveAsync();
    }

    public async Task<IEnumerable<BlogPostGetDTO>> GetAsync()
    {
        //var blogPosts = await _repository.GetAllAsync();
      
        var posts = await _repository.GetAll() 
                                 .Include(x => x.Category) 
                                 .ToListAsync();


        return _mapper.Map<IEnumerable<BlogPostGetDTO>>(posts);
    }

    public async Task<BlogPostGetDTO> GetByIdAsync(int id)
    {

        var blogPost = await _repository.GetByIdAsync(id);

        if (blogPost == null)
            throw new Exception("Bele bir post tapılmadı"); 

        return _mapper.Map<BlogPostGetDTO>(blogPost);
    }

    public async Task UpdateAsync(BlogPostUpdateDTO dto)
    {
        var existingPost = await _repository.GetByIdAsync(dto.Id);

        if (existingPost == null)
            throw new Exception("Yenilemek istediyiniz post tapılmadı");

        
        _mapper.Map(dto, existingPost);

        _repository.Update(existingPost);
        await _repository.SaveAsync();
    }
}
