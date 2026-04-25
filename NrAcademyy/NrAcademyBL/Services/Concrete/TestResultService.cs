using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyDAL.Context;


namespace NrAcademyBL.Services.Concrete;

public class TestResultService : ITestResultService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TestResultService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TestResultItemDto>> GetAllAsync()
    {
        var data = await _context.TestResults.ToListAsync();
        return _mapper.Map<IEnumerable<TestResultItemDto>>(data);
    }

    public async Task<TestResultItemDto> GetByIdAsync(int id)
    {
        var data = await _context.TestResults.FindAsync(id);
        if (data == null) throw new Exception("Nəticə tapılmadı");
        return _mapper.Map<TestResultItemDto>(data);
    }

    public async Task CreateAsync(TestResultCreateDto dto)
    {
        var entity = _mapper.Map<TestResult>(dto);
        await _context.TestResults.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TestResults.FindAsync(id);
        if (entity == null) throw new Exception("Nəticə tapılmadı");

        _context.TestResults.Remove(entity);
        await _context.SaveChangesAsync();
    }
}