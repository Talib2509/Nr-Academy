using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyDAL.Repositories; // Real repository class
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class TeacherServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly TeacherService _teacherService;
    private readonly IMapper _mapper;
    private readonly TeacherRepository _teacherRepo;

    public TeacherServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TeacherService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Real Repository (İnteqrasiya testi üçün)
        _teacherRepo = new TeacherRepository(_dbContext);

        // 3. Servisin inisializasiyası
        _teacherService = new TeacherService(_teacherRepo, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Successfully_Add_Teacher()
    {
        // Arrange
        var dto = new TeacherCreateDTO
        {
            Name = "Mirtalib",
            Bio = "Senior .NET Developer",
            ImageUrl = "test-image.jpg" // ƏLAVƏ EDİLDİ
        };

        // Act
        await _teacherService.CreateAsync(dto);
        await _dbContext.SaveChangesAsync();

        // Assert
        var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(t => t.Name == "Mirtalib");
        Assert.NotNull(teacher);
    }

    public async Task GetAllAsync_Should_Return_List_Of_Teachers()
    {
        // Arrange
        var teachers = new List<Teacher>
    {
        new Teacher { Name = "Alizamin", Bio = "Bio 1", ImageUrl = "img1.jpg" }, // DƏYƏRLƏR VERİLDİ
        new Teacher { Name = "Admin", Bio = "Bio 2", ImageUrl = "img2.jpg" }     // DƏYƏRLƏR VERİLDİ
    };
        await _dbContext.Teachers.AddRangeAsync(teachers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teacherService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
    }

    [Fact]
    public async Task GetByIdAsync_When_Teacher_Exists_Should_Return_Dto()
    {
        // Arrange
        var teacher = new Teacher { Name = "Test" };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teacherService.GetByIdAsync(teacher.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_When_Not_Found_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _teacherService.GetByIdAsync(999));
        Assert.Equal("Teacher not found", ex.Message);
    }

    [Fact]
    public async Task UpdateAsync_Should_Change_Teacher_Information()
    {
        // Arrange
        var teacher = new Teacher { Name = "Köhnə Ad", Bio = "Köhnə Bio", ImageUrl = "old.jpg" };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        // DTO-da da bu sahələr varsa, onları da doldurun
        var updateDto = new TeacherUpdateDTO { Name = "Yeni Ad", Bio = "Yeni Bio", ImageUrl = "new.jpg" };

        // Act
        await _teacherService.UpdateAsync(teacher.Id, updateDto);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updated = await _dbContext.Teachers.FindAsync(teacher.Id);
        Assert.Equal("Yeni Ad", updated!.Name);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Teacher_From_Db()
    {
        // Arrange
        var teacher = new Teacher { Name = "Silinəcək", Bio = "Temp Bio", ImageUrl = "temp.jpg" };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        // Act
        await _teacherService.DeleteAsync(teacher.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        var result = await _dbContext.Teachers.FindAsync(teacher.Id);
        Assert.Null(result);
    }
}