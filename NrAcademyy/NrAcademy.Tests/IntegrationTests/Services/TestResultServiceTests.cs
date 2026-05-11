using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.TestResultDTO;
using NrAcademyBL.Exceptions.TestResult;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Repositories;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class TestResultServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly TestResultService _testResultService;
    private readonly IMapper _mapper;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly ITestResultRepository _testResultRepository;

    public TestResultServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TestResultService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Cache Mock (VACİB)
        _cacheMock = new Mock<ICacheService>();

        // Testlərdə keşin hər zaman boş qayıtmasını təmin edirik ki, bazaya müraciət etsin
        _cacheMock.Setup(x => x.GetAsync<List<TestResultItemDto>>(It.IsAny<string>()))
                  .ReturnsAsync((List<TestResultItemDto>)null);

        // 3. Repository-ni başlat
        _testResultRepository = new TestResultRepository(_context);

        // 4. Servisi 3 parametr ilə başlat
        //_testResultService = new TestResultService(_testResultRepository, _mapper, _cacheMock.Object);
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
        //await _testResultService.CreateAsync(dto);

        // Assert
        var result = await _context.TestResults.FirstOrDefaultAsync(r => r.Score == 85);
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
        await _context.TestResults.AddRangeAsync(results);
        await _context.SaveChangesAsync();

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
        await _context.TestResults.AddAsync(resultEntity);
        await _context.SaveChangesAsync();

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
        // Tip olaraq Exception yox, öz yazdığınız TestResultException-ı gözləyin
        await Assert.ThrowsAsync<TestResultException>(() => _testResultService.GetByIdAsync(999));
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_From_Database()
    {
        // Arrange
        var resultEntity = new TestResult { Score = 10, TestId = 1, UserId = 1 };
        await _context.TestResults.AddAsync(resultEntity);
        await _context.SaveChangesAsync();

        // Act
        await _testResultService.DeleteAsync(resultEntity.Id);

        // Assert
        var exists = await _context.TestResults.AnyAsync(r => r.Id == resultEntity.Id);
        Assert.False(exists);
    }
}