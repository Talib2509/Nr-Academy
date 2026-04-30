using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using Xunit;
using static NrAcademyBL.DTOs.AuthDTO.TestDTO;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class TestServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly TestService _testService;
    private readonly IMapper _mapper;

    public TestServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TestService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        _testService = new TestService(_dbContext, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Test_To_Database()
    {
        // Arrange
        var dto = new TestCreateDto
        {
            Title = "Unit Testing Testi",
            Description = "Xunit mövzusunda kiçik imtahan"
        };

        // Act
        await _testService.CreateAsync(dto);

        // Assert
        var testInDb = await _dbContext.Tests.FirstOrDefaultAsync(t => t.Title == dto.Title);
        Assert.NotNull(testInDb);
       
    }
    [Fact]
    public async Task GetAllAsync_Should_Return_All_Tests()
    {
        // Arrange
        // Bazanı təmizlədiyinizdən əmin olun (əgər BaseIntegrationTest-də edilmirsə)
        _dbContext.Tests.RemoveRange(_dbContext.Tests);
        await _dbContext.SaveChangesAsync();

        var tests = new List<Test>
    {
        new Test { Title = "Test 1", CourseId = 1, PassingScore = 50 },
        new Test { Title = "Test 2", CourseId = 1, PassingScore = 60 }
    };

        await _dbContext.Tests.AddRangeAsync(tests);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _testService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // İndi bazada 2 element olduğu üçün keçəcək
    }

    [Fact]
    public async Task GetByIdAsync_When_Not_Exists_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _testService.GetByIdAsync(9999));
        Assert.Equal("Test tapılmadı", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Data_In_Db()
    {
        // Arrange
        var test = new Test { Title = "Köhnə Başlıq"};
        await _dbContext.Tests.AddAsync(test);
        await _dbContext.SaveChangesAsync();

        var updateDto = new TestUpdateDto
        {
            Id = test.Id,
            Title = "Yeni Başlıq",
            Description = "Yeni"
        };

        // Act
        await _testService.UpdateAsync(updateDto);

        // Assert
        var updated = await _dbContext.Tests.FindAsync(test.Id);
        Assert.Equal("Yeni Başlıq", updated!.Title);
      
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Test_From_Db()
    {
        // Arrange
        var test = new Test { Title = "Silinməli olan Test" };
        await _dbContext.Tests.AddAsync(test);
        await _dbContext.SaveChangesAsync();

        // Act
        await _testService.DeleteAsync(test.Id);

        // Assert
        var exists = await _dbContext.Tests.AnyAsync(t => t.Id == test.Id);
        Assert.False(exists);
    }
}