using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.AnswerDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Repositories;
using Xunit;


namespace NrAcademy.Tests.IntegrationTests.Services;

public class AnswerServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly AnswerService _answerService;
    private readonly IMapper _mapper;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly IAnswerRepository _answerRepository;

    public AnswerServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AnswerService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Cache Mock obyekti
        _cacheMock = new Mock<ICacheService>();

        // 3. ƏSAS HİSSƏ: Repository-ni burada mütləq yaradın!
        // BaseIntegrationTest-dən gələn _context-i bura ötürürük.
        _answerRepository = new AnswerRepository(_context);

        // 4. Servisi artıq dolu parametrlarla başladırıq
        _answerService = new AnswerService(_answerRepository, _mapper, _cacheMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Answer_To_Database()
    {
        // Arrange
        var dto = new NrAcademyBL.DTOs.AnswerDTO.AnswerCreateDto
        {
            Text = "Test Answer",
            IsCorrect = true,
            QuestionId = 1 // Öncədən Question yaratmaq daha yaxşı olar, amma sadəlik üçün belə qalsın
        };

        // Act
        await _answerService.CreateAsync(dto);

        // Assert
        var answer = await _context.Answers.FirstOrDefaultAsync(a => a.AnswerText == "Test Answer");
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
        await _context.Answers.AddRangeAsync(answers);
        await _context.SaveChangesAsync();

        // Act
        var result = await _answerService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_Exception_When_Not_Found()
    {
        // Act & Assert
        // Exception yerinə NrAcademyBL.Exceptions.Answer.AnswerException istifadə edin
        var exception = await Assert.ThrowsAsync<NrAcademyBL.Exceptions.Answer.AnswerException>(
            () => _answerService.GetByIdAsync(999)
        );

        // Xəta mesajının doğruluğunu yoxlayın (servisdən gələn mesajla eyni olmalıdır)
        Assert.Equal("ID: 999 olan cavab tapılmadı.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Existing_Answer()
    {
        // Arrange
        var answer = new Answer { AnswerText = "Old Text", IsCorrect = false, QuestionId = 1 };
        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();

        var updateDto = new AnswerUpdateDto
        {
            Id = answer.Id,
            Text = "New Text",
            IsCorrect = true
        };

        // Act
        await _answerService.UpdateAsync(updateDto);

        // Assert
        var updated = await _context.Answers.FindAsync(answer.Id);
        Assert.Equal("New Text", updated!.AnswerText);
        Assert.True(updated.IsCorrect);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Answer_From_Db()
    {
        // Arrange
        var answer = new Answer { AnswerText = "To Be Deleted", QuestionId = 1 };
        await _context.Answers.AddAsync(answer);
        await _context.SaveChangesAsync();

        // Act
        await _answerService.DeleteAsync(answer.Id);

        // Assert
        var deleted = await _context.Answers.FindAsync(answer.Id);
        Assert.Null(deleted);
    }
}