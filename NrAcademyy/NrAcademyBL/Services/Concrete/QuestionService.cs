using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyDAL.Context;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;

namespace NrAcademyBL.Services.Concrete;

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public QuestionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuestionItemDto>> GetAllAsync()
    {
        var data = await _context.Question.ToListAsync();
        return _mapper.Map<IEnumerable<QuestionItemDto>>(data);
    }

    public async Task<QuestionItemDto> GetByIdAsync(int id)
    {
        var data = await _context.Question.FindAsync(id);
        if (data == null) throw new Exception("Sual tapılmadı");
        return _mapper.Map<QuestionItemDto>(data);
    }

    public async Task CreateAsync(QuestionCreateDto dto)
    {
        var entity = _mapper.Map<Question>(dto);
        await _context.Question.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(QuestionUpdateDto dto)
    {
        var entity = await _context.Question.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Sual tapılmadı");

        _mapper.Map(dto, entity);
        _context.Question.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Question.FindAsync(id);
        if (entity == null) throw new Exception("Sual tapılmadı");

        _context.Question.Remove(entity);
        await _context.SaveChangesAsync();
    }
}