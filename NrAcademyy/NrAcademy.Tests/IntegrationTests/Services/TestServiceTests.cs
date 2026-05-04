using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.TestDTO;
using NrAcademyBL.Extensions.Caching;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Repositories;
using Xunit;
using static NrAcademyBL.DTOs.TestDTO.TestDTO;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class TestServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly TestService _testService;
    private readonly IMapper _mapper;
    private readonly Mock<ICacheService> _cacheMock;
    private readonly ITestRepository _testRepository;

    public TestServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TestService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Cache Mock (VACİB)
        _cacheMock = new Mock<ICacheService>();

        // 3. Repository-ni başlat (Null xətası almamalıdır)
        _testRepository = new TestRepository(_context);

        // 4. Servisi 3 arqumentlə yaradın
        _testService = new TestService(_testRepository, _mapper, _cacheMock.Object);
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
        var testInDb = await _context.Tests.FirstOrDefaultAsync(t => t.Title == dto.Title);
        Assert.NotNull(testInDb);
       
    }
    [Fact]
    
    public async Task GetAllAsync_Should_Return_All_Tests()
    {
        // Arrange
        _context.Tests.RemoveRange(_context.Tests);

        // Əgər bazada CourseId məcburidirsə, bir kurs yaradın:
        var course = new Course { Title = "C# Course", Description = "Desc", ImageUrl = "c.jpg" };
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        var tests = new List<Test>
    {
        new Test { Title = "Test 1", CourseId = course.Id, PassingScore = 50 },
        new Test { Title = "Test 2", CourseId = course.Id, PassingScore = 60 }
    };

        await _context.Tests.AddRangeAsync(tests);
        await _context.SaveChangesAsync();

        // Act
        var result = await _testService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }
    [Fact]
    public async Task GetByIdAsync_When_Not_Exists_Should_Throw_Exception()
    {
        // Act & Assert
        // Əgər TestException xüsusi bir tipdirsə, <Exception> yerinə onu yazın
        await Assert.ThrowsAsync<NrAcademyBL.Exceptions.Test.TestException>(
            () => _testService.GetByIdAsync(9999)
        );
    }

    [Fact]
    public async Task UpdateAsync_Should_Modify_Data_In_Db()
    {
        // Arrange
        var test = new Test { Title = "Köhnə Başlıq"};
        await _context.Tests.AddAsync(test);
        await _context.SaveChangesAsync();

        var updateDto = new TestUpdateDto
        {
            Id = test.Id,
            Title = "Yeni Başlıq",
            Description = "Yeni"
        };

        // Act
        await _testService.UpdateAsync(updateDto);

        // Assert
        var updated = await _context.Tests.FindAsync(test.Id);
        Assert.Equal("Yeni Başlıq", updated!.Title);
      
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Test_From_Db()
    {
        // Arrange
        var test = new Test { Title = "Silinməli olan Test" };
        await _context.Tests.AddAsync(test);
        await _context.SaveChangesAsync();

        // Act
        await _testService.DeleteAsync(test.Id);

        // Assert
        var exists = await _context.Tests.AnyAsync(t => t.Id == test.Id);
        Assert.False(exists);
    }
}