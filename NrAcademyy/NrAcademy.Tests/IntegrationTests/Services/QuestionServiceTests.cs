using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using Xunit;
using static NrAcademyBL.DTOs.AuthDTO.QuestionDTO;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class QuestionServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly QuestionService _questionService;
    private readonly IMapper _mapper;

    public QuestionServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(QuestionService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        _questionService = new QuestionService(_dbContext, _mapper);
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
        var question = await _dbContext.Question.FirstOrDefaultAsync(q => q.QuestionText == dto.Text);
        Assert.NotNull(question);
        Assert.Equal("SingleChoice", question.QuestionType);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Stored_Questions()
    {
        // Arrange
        // Əvvəlki testlərdən qalan datanı təmizləmək üçün (opsional)
        _dbContext.Question.RemoveRange(_dbContext.Question);
        await _dbContext.SaveChangesAsync();

        var questions = new List<Question>
    {
        new Question { QuestionText = "Q1", QuestionType = "General", TestId = 1 },
        new Question { QuestionText = "Q2", QuestionType = "Technical", TestId = 1 }
    };
        await _dbContext.Question.AddRangeAsync(questions);
        await _dbContext.SaveChangesAsync();

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
        await _dbContext.Question.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        var updateDto = new QuestionUpdateDto
        {
            Id = question.Id,
            Text = "Yeni Sual",
            QuestionType = "TypeB" // Əgər DTO-da varsa doldurun
        };

        // Act
        await _questionService.UpdateAsync(updateDto);

        // Assert
        var updated = await _dbContext.Question.FindAsync(question.Id);
        Assert.Equal("Yeni Sual", updated!.QuestionText);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Question_From_Database()
    {
        // Arrange
        var question = new Question { QuestionText = "Silinəcək", QuestionType = "Temp", TestId = 1 };
        await _dbContext.Question.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        // Act
        await _questionService.DeleteAsync(question.Id);

        // Assert
        var result = await _dbContext.Question.FindAsync(question.Id);
        Assert.Null(result);
    }
    [Fact]
    public async Task GetByIdAsync_With_Invalid_Id_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _questionService.GetByIdAsync(999));
        Assert.Equal("Sual tapılmadı", ex.Message);
    }

 
}
