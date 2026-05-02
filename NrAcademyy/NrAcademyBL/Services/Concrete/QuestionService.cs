using Abp.Runtime.Caching;
using AutoMapper;

using Microsoft.EntityFrameworkCore;

using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Exceptions.Question;

using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using NrAcademyDAL.Repositories;

using static NrAcademyBL.DTOs.QuestionDTO.QuestionDTO;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;


namespace NrAcademyBL.Services.Concrete;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public QuestionService(IQuestionRepository questionRepository, IMapper mapper, ICacheService cache)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<QuestionItemDto>> GetAllAsync()
    {
        var key = "questions_all";

        var cached = await _cache.GetAsync<List<QuestionItemDto>>(key);
        if (cached != null)
            return cached;

        var data = await _questionRepository.GetAllAsync();
        var mapped = _mapper.Map<List<QuestionItemDto>>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(5));

        return mapped;
    }

    public async Task<QuestionItemDto> GetByIdAsync(int id)
    {
        var key = $"question_{id}";

        var cached = await _cache.GetAsync<QuestionItemDto>(key);
        if (cached != null)
            return cached;

        var data = await _questionRepository.GetByIdAsync(id);
        if (data == null)
            throw QuestionException.NotFound(id);

        var mapped = _mapper.Map<QuestionItemDto>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(5));

        return mapped;
    }

    public async Task CreateAsync(QuestionCreateDto dto)
    {
        var entity = _mapper.Map<Question>(dto);
        await _questionRepository.AddAsync(entity);

        await _cache.RemoveAsync("questions_all");
    }

    public async Task UpdateAsync(QuestionUpdateDto dto)
    {
        var entity = await _questionRepository.GetByIdAsync(dto.Id);
        if (entity == null)
            throw QuestionException.NotFound(dto.Id);

        _mapper.Map(dto, entity);
        _questionRepository.Update(entity);

        await _cache.RemoveAsync("questions_all");
        await _cache.RemoveAsync($"question_{dto.Id}");
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _questionRepository.GetByIdAsync(id);
        if (entity == null)
            throw QuestionException.NotFound(id);

        _questionRepository.Delete(entity);

        await _cache.RemoveAsync("questions_all");
        await _cache.RemoveAsync($"question_{id}");
    }
}