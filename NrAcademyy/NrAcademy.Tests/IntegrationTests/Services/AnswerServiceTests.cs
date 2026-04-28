using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using Xunit;
using static NrAcademyBL.DTOs.AuthDTO.AnswerDTO;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class AnswerServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly AnswerService _answerService;
    private readonly IMapper _mapper;

    public AnswerServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            // Real Mapping Profile-larını əlavə et (məsələn AnswerProfile)
            cfg.AddMaps(typeof(AnswerService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        _answerService = new AnswerService(_dbContext, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Answer_To_Database()
    {
        // Arrange
        var dto = new AnswerCreateDto
        {
            Text = "Test Answer",
            IsCorrect = true,
            QuestionId = 1 // Öncədən Question yaratmaq daha yaxşı olar, amma sadəlik üçün belə qalsın
        };

        // Act
        await _answerService.CreateAsync(dto);

        // Assert
        var answer = await _dbContext.Answers.FirstOrDefaultAsync(a => a.AnswerText == "Test Answer");
        Assert.NotNull(answer);
        Assert.Equal(dto.IsCorrect, answer.IsCorrect);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Answers()
    {
        // Arrange
        var answers = new List<Answer>
        {
            new Answer { AnswerText = "A1", IsCorrect = false, QuestionId = 1 },
            new Answer { AnswerText = "A2", IsCorrect = true, QuestionId = 1 }
        };
        await _dbContext.Answers.AddRangeAsync(answers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _answerService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_Exception_When_Not_Found()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _answerService.GetByIdAsync(999));
        Assert.Equal("Tapılmadı", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Existing_Answer()
    {
        // Arrange
        var answer = new Answer { AnswerText = "Old Text", IsCorrect = false, QuestionId = 1 };
        await _dbContext.Answers.AddAsync(answer);
        await _dbContext.SaveChangesAsync();

        var updateDto = new AnswerUpdateDto
        {
            Id = answer.Id,
            Text = "New Text",
            IsCorrect = true
        };

        // Act
        await _answerService.UpdateAsync(updateDto);

        // Assert
        var updated = await _dbContext.Answers.FindAsync(answer.Id);
        Assert.Equal("New Text", updated!.AnswerText);
        Assert.True(updated.IsCorrect);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Answer_From_Db()
    {
        // Arrange
        var answer = new Answer { AnswerText = "To Be Deleted", QuestionId = 1 };
        await _dbContext.Answers.AddAsync(answer);
        await _dbContext.SaveChangesAsync();

        // Act
        await _answerService.DeleteAsync(answer.Id);

        // Assert
        var deleted = await _dbContext.Answers.FindAsync(answer.Id);
        Assert.Null(deleted);
    }
}