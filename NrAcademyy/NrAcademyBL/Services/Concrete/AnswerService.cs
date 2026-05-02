using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Exceptions.Answer;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using NrAcademyDAL.Repositories;

using static NrAcademyBL.DTOs.AnswerDTO.AnswerDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;

namespace NrAcademyBL.Services.Concrete;

public class AnswerService : IAnswerService
{
    private readonly IAnswerRepository _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public AnswerService(IAnswerRepository context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<AnswerItemDto>> GetAllAsync()
    {
        var key = "answers_all";

        var cached = await _cache.GetAsync<List<AnswerItemDto>>(key);
        if (cached != null)
            return cached;

        var data = await _context.GetAllAsync();
        var mapped = _mapper.Map<List<AnswerItemDto>>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

        return mapped;
    }

    public async Task<AnswerItemDto> GetByIdAsync(int id)
    {
        var key = $"answer_{id}";

        var cached = await _cache.GetAsync<AnswerItemDto>(key);
        if (cached != null)
            return cached;

        var data = await _context.GetByIdAsync(id);
        if (data == null)
            throw AnswerException.NotFound(id);

        var mapped = _mapper.Map<AnswerItemDto>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(10));

        return mapped;
    }

    public async Task CreateAsync(AnswerCreateDto dto)
    {
        var entity = _mapper.Map<Answer>(dto);
        await _context.AddAsync(entity);

        await _cache.RemoveAsync("answers_all");
    }

    public async Task UpdateAsync(AnswerUpdateDto dto)
    {
        var entity = await _context.GetByIdAsync(dto.Id);
        if (entity == null)
            throw AnswerException.NotFound(dto.Id);

        _mapper.Map(dto, entity);
        _context.Update(entity);

        await _cache.RemoveAsync("answers_all");
        await _cache.RemoveAsync($"answer_{dto.Id}");
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.GetByIdAsync(id);
        if (entity == null)
            throw AnswerException.NotFound(id);

        _context.Delete(entity);

        await _cache.RemoveAsync("answers_all");
        await _cache.RemoveAsync($"answer_{id}");
    }
}