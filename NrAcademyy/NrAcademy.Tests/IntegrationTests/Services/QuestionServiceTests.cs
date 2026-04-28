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
            Text = "C# nədir?"
           
        };

        // Act
        await _questionService.CreateAsync(dto);

        // Assert
        var question = await _dbContext.Question.FirstOrDefaultAsync(q => q.QuestionText == dto.Text);
        Assert.NotNull(question);
       
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Stored_Questions()
    {
        // Arrange
        var questions = new List<Question>
        {
            new Question { QuestionText = "Q1"},
            new Question { QuestionText = "Q2"}
        };
        await _dbContext.Question.AddRangeAsync(questions);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _questionService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_With_Invalid_Id_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _questionService.GetByIdAsync(999));
        Assert.Equal("Sual tapılmadı", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Effectively_Change_Data()
    {
        // Arrange
        var question = new Question {QuestionText = "Köhnə Sual"};
        await _dbContext.Question.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        var updateDto = new QuestionUpdateDto
        {
            Id = question.Id,
            Text = "Yeni Sual"
          
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
        var question = new Question { QuestionText = "Silinəcək"};
        await _dbContext.Question.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        // Act
        await _questionService.DeleteAsync(question.Id);

        // Assert
        var result = await _dbContext.Question.FindAsync(question.Id);
        Assert.Null(result);
    }
}
