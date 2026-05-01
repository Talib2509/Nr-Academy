using AutoMapper;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using static NrAcademyBL.DTOs.AuthDTO.TestDTO;

namespace NrAcademyBL.Services.Concrete;

public class TestService : ITestService
{
    private readonly ITestRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public TestService(
        ITestRepository repository,
        IMapper mapper,
        ICacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<List<TestItemDto>> GetAllAsync()
    {
        var key = "tests_all";

        var cached = await _cache.GetAsync<List<TestItemDto>>(key);
        if (cached != null)
            return cached;

        var data = await _repository.GetAllAsync();
        var mapped = _mapper.Map<List<TestItemDto>>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

        return mapped;
    }

    public async Task<TestItemDto> GetByIdAsync(int id)
    {
        var key = $"test_{id}";

        var cached = await _cache.GetAsync<TestItemDto>(key);
        if (cached != null)
            return cached;

        var data = await _repository.GetByIdAsync(id);

        if (data == null)
            throw new Exception("Test tapılmadı");

        var mapped = _mapper.Map<TestItemDto>(data);

        await _cache.SetAsync(key, mapped, TimeSpan.FromMinutes(30));

        return mapped;
    }

    public async Task CreateAsync(TestCreateDto dto)
    {
        var entity = _mapper.Map<Test>(dto);
        await _repository.AddAsync(entity);

        await _cache.RemoveAsync("tests_all");
    }

    public async Task UpdateAsync(TestUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(dto.Id);
        if (entity == null)
            throw new Exception("Test tapılmadı");

        _mapper.Map(dto, entity);
        _repository.Update(entity);

        await _cache.RemoveAsync("tests_all");
        await _cache.RemoveAsync($"test_{dto.Id}");
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new Exception("Test tapılmadı");

        _repository.Delete(entity);

        await _cache.RemoveAsync("tests_all");
        await _cache.RemoveAsync($"test_{id}");
    }
}