using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.QuestionDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Repositories;
using Xunit;


namespace NrAcademy.Tests.IntegrationTests.Services;

public class QuestionServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly QuestionService _questionService;
    private readonly IMapper _mapper;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly IQuestionRepository _questionRepository; // Repository əlavə edildi

    public QuestionServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(QuestionService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Cache Mock-u yaradın (BU VACİBDİR)
        _cacheMock = new Mock<ICacheService>();

        // 3. Repository-ni başladın
        _questionRepository = new QuestionRepository(_context);

        // 4. Servisi bütün parametrlərlə başladın
        _questionService = new QuestionService(_questionRepository, _mapper, _cacheMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Save_Question_To_Db()
    {
        // Arrange
        var dto = new QuestionCreateDto
        {
            Text = "C# nədir?",
            QuestionType = "SingleChoice", // ƏLAVƏ EDİLDİ
            TestId = 1                     // ƏLAVƏ EDİLDİ
        };

        // Act
        await _questionService.CreateAsync(dto);

        // Assert
        var question = await _context.Question.FirstOrDefaultAsync(q => q.QuestionText == dto.Text);
        Assert.NotNull(question);
        Assert.Equal("SingleChoice", question.QuestionType);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Stored_Questions()
    {
        // Arrange
        // Əvvəlki testlərdən qalan datanı təmizləmək üçün (opsional)
        _context.Question.RemoveRange(_context.Question);
        await _context.SaveChangesAsync();

        var questions = new List<Question>
    {
        new Question { QuestionText = "Q1", QuestionType = "General", TestId = 1 },
        new Question { QuestionText = "Q2", QuestionType = "Technical", TestId = 1 }
    };
        await _context.Question.AddRangeAsync(questions);
        await _context.SaveChangesAsync();

        // Act
        var result = await _questionService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Effectively_Change_Data()
    {
        // Arrange
        var question = new Question { QuestionText = "Köhnə Sual", QuestionType = "TypeA", TestId = 1 };
        await _context.Question.AddAsync(question);
        await _context.SaveChangesAsync();

        var updateDto = new QuestionUpdateDto
        {
            Id = question.Id,
            Text = "Yeni Sual",
            QuestionType = "TypeB" // Əgər DTO-da varsa doldurun
        };

        // Act
        await _questionService.UpdateAsync(updateDto);

        // Assert
        var updated = await _context.Question.FindAsync(question.Id);
        Assert.Equal("Yeni Sual", updated!.QuestionText);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Question_From_Database()
    {
        // Arrange
        var question = new Question { QuestionText = "Silinəcək", QuestionType = "Temp", TestId = 1 };
        await _context.Question.AddAsync(question);
        await _context.SaveChangesAsync();

        // Act
        await _questionService.DeleteAsync(question.Id);

        // Assert
        var result = await _context.Question.FindAsync(question.Id);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByIdAsync_With_Invalid_Id_Should_Throw_Exception()
    {
        // Act & Assert
        // Exception yerinə NrAcademyBL.Exceptions.Question.QuestionException yazın
        var ex = await Assert.ThrowsAsync<NrAcademyBL.Exceptions.Question.QuestionException>(
            () => _questionService.GetByIdAsync(999)
        );

        // Servisdən gələn real mesajı yoxlayın
        Assert.Equal("ID: 999 olan sual tapılmadı.", ex.Message);
    }


}
