
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.BlogPostDTO;
using NrAcademyBL.Extensions;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete;

public class BlogPostService : IBlogPostService
{
    private readonly IBlogPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public BlogPostService(IBlogPostRepository repository, IMapper mapper, ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task CreateAsync(BlogPostCreateDTO dto, string rootPath)
    {
        var newBlogPost = _mapper.Map<BlogPost>(dto);

        if (dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsValidType("image"))
                throw new Exception("Yalnız şəkil formatı yüklənə bilər.");

            if (!dto.ImageFile.IsValidSize(5000)) // ~5MB
                throw new Exception("Şəkil həcmi 5MB-dan çox ola bilməz.");

            string uploadsFolder = Path.Combine(rootPath, "uploads", "blogposts");
            newBlogPost.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
        }

        await _repository.AddAsync(newBlogPost);
        await _cache.RemoveAsync("blogposts_all");
    }

    public async Task DeleteAsync(int id, string rootPath)
    {
        var blogPost = await _repository.GetByIdAsync(id);

        if (blogPost == null)
            throw new Exception("Silmək istədiyiniz post tapılmadı");

        if (!string.IsNullOrEmpty(blogPost.ImageUrl))
        {
            FileExtensions.DeleteFile(Path.GetFileName(blogPost.ImageUrl), rootPath, "uploads", "blogposts");
        }

        _repository.Delete(blogPost);

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
            throw new Exception("Belə bir post tapılmadı");

        var mapped = _mapper.Map<BlogPostGetDTO>(blogPost);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

        return mapped;
    }

    public async Task UpdateAsync(BlogPostUpdateDTO dto, string rootPath)
    {
        var existingPost = await _repository.GetByIdAsync(dto.Id);

        if (existingPost == null)
            throw new Exception("Yeniləmək istədiyiniz post tapılmadı");

        if (dto.ImageFile != null)
        {
            if (!dto.ImageFile.IsValidType("image"))
                throw new Exception("Yalnız şəkil formatı yüklənə bilər.");

            if (!dto.ImageFile.IsValidSize(5000))
                throw new Exception("Şəkil həcmi 5MB-dan çox ola bilməz.");

            if (!string.IsNullOrEmpty(existingPost.ImageUrl))
            {
                FileExtensions.DeleteFile(Path.GetFileName(existingPost.ImageUrl), rootPath, "uploads", "blogposts");
            }

            string uploadsFolder = Path.Combine(rootPath, "uploads", "blogposts");
            existingPost.ImageUrl = await dto.ImageFile.UploadAsync(uploadsFolder);
        }

        _mapper.Map(dto, existingPost);
        _repository.Update(existingPost);

        await _cache.RemoveAsync("blogposts_all");
        await _cache.RemoveAsync($"blogpost_{dto.Id}");
    }
}