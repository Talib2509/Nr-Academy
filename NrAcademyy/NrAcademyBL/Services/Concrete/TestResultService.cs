
﻿using AutoMapper;
using NrAcademyBL.DTOs.TestResultDTO
﻿
using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Exceptions.TestResult;

using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete;

public class TestResultService : ITestResultService
{
    private readonly ITestResultRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public TestResultService(
        ITestResultRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<TestResultItemDto>> GetAllAsync()
    {
        var key = "testresults_all";

        var cached = await _cache.GetAsync<List<TestResultItemDto>>(key);
        if (cached != null)
            return cached;

        var data = await _repository.GetAllAsync();
        var mapped = _mapper.Map<List<TestResultItemDto>>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

        return mapped;
    }

    public async Task<TestResultItemDto> GetByIdAsync(int id)
    {
        var key = $"testresult_{id}";

        var cached = await _cache.GetAsync<TestResultItemDto>(key);
        if (cached != null)
            return cached;

        var data = await _repository.GetByIdAsync(id);

        if (data == null)
            throw TestResultException.NotFound(id);

        var mapped = _mapper.Map<TestResultItemDto>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

        return mapped;
    }

    public async Task CreateAsync(TestResultCreateDto dto)
    {
        var entity = _mapper.Map<TestResult>(dto);
        await _repository.AddAsync(entity);

        await _cache.RemoveAsync("testresults_all");
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw TestResultException.NotFound(id);

        _repository.Delete(entity);

        await _cache.RemoveAsync("testresults_all");
        await _cache.RemoveAsync($"testresult_{id}");
    }
}