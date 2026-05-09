using AutoMapper;
using NrAcademyBL.DTOs.TestiomonialDTO;
using NrAcademyBL.Exceptions.Testimonial;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete;

public class TestimonialService(ITestimonialRepository _repository, IMapper _mapper, ICacheService _cache) : ITestimonialService
{
    private const string AllCacheKey = "testimonials_all";
    private static string ItemCacheKey(int id) => $"testimonial_{id}";
    public async Task CreateAsync(TestimonialCreateDto dto)
    {
         var entity = _mapper.Map<Testimonial>(dto);
        await _repository.AddAsync(entity);
        await _cache.RemoveAsync(AllCacheKey);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) throw TestimonialException.NotFound(id);

        _repository.Delete(entity);
        await _cache.RemoveAsync(AllCacheKey);
        await _cache.RemoveAsync(ItemCacheKey(id));
    }

    public async Task<List<TestimonialGetDto>> GetAllAsync()
    {
        var cached = await _cache.GetAsync<List<TestimonialGetDto>>(AllCacheKey);
        if (cached != null) return cached;

        var data = await _repository.GetAllAsync();
        var mapped = _mapper.Map<List<TestimonialGetDto>>(data);

        await _cache.SetAsync(AllCacheKey, mapped, TimeSpan.FromMinutes(30));
        return mapped;
    }

    public async Task<TestimonialGetDto> GetByIdAsync(int id)
    {
        var cached = await _cache.GetAsync<TestimonialGetDto>(ItemCacheKey(id));
        if (cached != null) return cached;

        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) throw TestimonialException.NotFound(id);

        var mapped = _mapper.Map<TestimonialGetDto>(entity);
        await _cache.SetAsync(ItemCacheKey(id), mapped, TimeSpan.FromMinutes(30));
        return mapped;
    }

    public async Task UpdateAsync(TestimonialUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity == null)
        throw TestimonialException.NotFound(dto.Id);

        entity.ReviewText = dto.ReviewText;
        entity.Rating = dto.Rating;

        _repository.Update(entity);
        await _cache.RemoveAsync(AllCacheKey);
        await _cache.RemoveAsync(ItemCacheKey(dto.Id));
    }
}
