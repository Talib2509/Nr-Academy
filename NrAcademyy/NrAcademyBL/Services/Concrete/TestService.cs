using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyDAL.Context;
using static NrAcademyBL.DTOs.AuthDTO.TestDTO;

namespace NrAcademyBL.Services.Concrete;

public class TestService : ITestService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TestService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestItemDto>> GetAllAsync()
    {
        var data = await _context.Tests.ToListAsync();
        return _mapper.Map<IEnumerable<TestItemDto>>(data);
    }

    public async Task<TestItemDto> GetByIdAsync(int id)
    {
        var data = await _context.Tests.FindAsync(id);
        if (data == null) throw new Exception("Test tapılmadı");
        return _mapper.Map<TestItemDto>(data);
    }

    public async Task CreateAsync(TestCreateDto dto)
    {
        var entity = _mapper.Map<Test>(dto);
        await _context.Tests.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TestUpdateDto dto)
    {
        var entity = await _context.Tests.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Test tapılmadı");

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Tests.FindAsync(id);
        if (entity == null) throw new Exception("Test tapılmadı");

        _context.Tests.Remove(entity);
        await _context.SaveChangesAsync();
    }
}