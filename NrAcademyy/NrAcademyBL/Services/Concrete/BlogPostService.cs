using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete;
public class BlogPostService: IBlogPostService
{
    private readonly IBlogPostRepository _repository;
    private readonly IMapper _mapper;

    private readonly ICacheService _cache;
    public BlogPostService(IBlogPostRepository _repository, IMapper _mapper, ICacheService cache)
    {
        _repository = _repository;
        _mapper = _mapper;
        _cache = cache;
    }
    public async Task CreateAsync(BlogPostCreateDTO dto)
    {
        var newBlogPost = _mapper.Map<BlogPost>(dto);

        await _repository.AddAsync(newBlogPost);
        

        await _cache.RemoveAsync("blogposts_all");
    }

    public async Task DeleteAsync(int id)
    {
        var blogPosts = await _repository.GetByIdAsync(id);

        if (blogPosts == null)
            throw new Exception("Silmek istediyiniz Post Tapilmadi");

        _repository.Delete(blogPosts);
    

        await _cache.RemoveAsync("blogposts_all");
        await _cache.RemoveAsync($"blogpost_{id}");
    }

    public async Task<List<BlogPostGetDTO>> GetAsync()
    {
        var key = "blogposts_all";

        var cached = await _cache.GetAsync<List<BlogPostGetDTO>>(key);
        if (cached != null)
            return cached;

        var posts = await _repository.GetAll()
                                     .Include(x => x.Category)
                                     .ToListAsync();

        var mapped = _mapper.Map<List<BlogPostGetDTO>>(posts);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

        return mapped;
    }

    public async Task<BlogPostGetDTO> GetByIdAsync(int id)
    {
        var key = $"blogpost_{id}";

        var cached = await _cache.GetAsync<BlogPostGetDTO>(key);
        if (cached != null)
            return cached;

        var blogPost = await _repository.GetByIdAsync(id);

        if (blogPost == null)
            throw new Exception("Bele bir post tapılmadı");

        var mapped = _mapper.Map<BlogPostGetDTO>(blogPost);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

        return mapped;
    }

    public async Task UpdateAsync(BlogPostUpdateDTO dto)
    {
        var existingPost = await _repository.GetByIdAsync(dto.Id);

        if (existingPost == null)
            throw new Exception("Yenilemek istediyiniz post tapılmadı");

        _mapper.Map(dto, existingPost);

        _repository.Update(existingPost);
       

        await _cache.RemoveAsync("blogposts_all");
        await _cache.RemoveAsync($"blogpost_{dto.Id}");
    }
} 
