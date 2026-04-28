using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class TestResultServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly TestResultService _testResultService;
    private readonly IMapper _mapper;

    public TestResultServiceTests(TestDbContextFactory factory) : base(factory)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TestResultService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        _testResultService = new TestResultService(_dbContext, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Save_Result_Correctly()
    {
        // Arrange
        var dto = new TestResultCreateDto
        {
            Score = 85,
            TestId = 1,
            AppUserId = 1 // Real bazada bu ID-li data yoxdursa, EnsureCreated buna icazə verməyə bilər
        };

        // Act
        await _testResultService.CreateAsync(dto);

        // Assert
        var result = await _dbContext.TestResults.FirstOrDefaultAsync(r => r.Score == 85);
        Assert.NotNull(result);
        Assert.Equal(1, result.TestId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_List()
    {
        // Arrange
        var results = new List<TestResult>
        {
            new TestResult { Score = 50, TestId = 1, UserId = 1 },
            new TestResult { Score = 90, TestId = 2, UserId = 1 }
        };
        await _dbContext.TestResults.AddRangeAsync(results);
        await _dbContext.SaveChangesAsync();

        // Act
        var data = await _testResultService.GetAllAsync();

        // Assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetByIdAsync_When_Result_Exists_Should_Return_Dto()
    {
        // Arrange
        var resultEntity = new TestResult { Score = 100, TestId = 1, UserId = 1 };
        await _dbContext.TestResults.AddAsync(resultEntity);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _testResultService.GetByIdAsync(resultEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.Score);
    }

    [Fact]
    public async Task GetByIdAsync_When_Not_Found_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _testResultService.GetByIdAsync(999));
        Assert.Equal("Nəticə tapılmadı", ex.Message);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_From_Database()
    {
        // Arrange
        var resultEntity = new TestResult { Score = 10, TestId = 1, UserId = 1 };
        await _dbContext.TestResults.AddAsync(resultEntity);
        await _dbContext.SaveChangesAsync();

        // Act
        await _testResultService.DeleteAsync(resultEntity.Id);

        // Assert
        var exists = await _dbContext.TestResults.AnyAsync(r => r.Id == resultEntity.Id);
        Assert.False(exists);
    }
}