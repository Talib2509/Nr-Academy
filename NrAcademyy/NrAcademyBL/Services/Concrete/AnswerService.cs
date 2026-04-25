using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyDAL.Context;
using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;

namespace NrAcademyBL.Services.Concrete;

public class AnswerService : IAnswerService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AnswerService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AnswerItemDto>> GetAllAsync()
    {
        var data = await _context.Answers.ToListAsync();
        return _mapper.Map<IEnumerable<AnswerItemDto>>(data);
    }

    public async Task<AnswerItemDto> GetByIdAsync(int id)
    {
        var data = await _context.Answers.FindAsync(id);
        if (data == null) throw new Exception("Tapılmadı");
        return _mapper.Map<AnswerItemDto>(data);
    }

    public async Task CreateAsync(AnswerCreateDto dto)
    {
        var entity = _mapper.Map<Answer>(dto);
        await _context.Answers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AnswerUpdateDto dto)
    {
        var entity = await _context.Answers.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Tapılmadı");

        _mapper.Map(dto, entity); // DTO-dakı məlumatları mövcud entity-yə kopyalayır
        _context.Answers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Answers.FindAsync(id);
        if (entity == null) throw new Exception("Tapılmadı");

        _context.Answers.Remove(entity);
        await _context.SaveChangesAsync();
    }
}