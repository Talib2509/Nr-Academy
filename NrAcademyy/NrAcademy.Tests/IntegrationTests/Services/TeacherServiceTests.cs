using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.TeacherDTOs;
using NrAcademyBL.Extensions.Caching;
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
    private readonly Mock<ICacheService> _cacheMock;

    public TeacherServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper konfiqurasiyası
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(TeacherService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Real Repository (İnteqrasiya testi üçün)
        _teacherRepo = new TeacherRepository(_context);
        _cacheMock = new Mock<ICacheService>();
        // 2. Cache Mock-u yaradın (BU VACİBDİR)
        _cacheMock = new Mock<ICacheService>();

        // 3. Servisin inisializasiyası
        _teacherService = new TeacherService(_teacherRepo, _mapper, _cacheMock.Object);
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
        await _teacherService.CreateAsync(dto, "wwwroot");
        await _context.SaveChangesAsync();

        // Assert
        var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Name == "Mirtalib");
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
        await _context.Teachers.AddRangeAsync(teachers);
        await _context.SaveChangesAsync();

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
        var teacher = new Teacher
        {
            Name = "Test Müəllim",
            Bio = "Bu sahə məcburidir, ona görə doldurulmalıdır.", // Xətanın həlli buradadır
            ImageUrl = "test.jpg" // Əgər bu da Required-dirsə, bunu da əlavə edin
        };

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync(); // Xəta burada baş verirdi

        // Act
        var result = await _teacherService.GetByIdAsync(teacher.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Müəllim", result.Name);
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
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        // DTO-da da bu sahələr varsa, onları da doldurun
        var updateDto = new TeacherUpdateDTO { Name = "Yeni Ad", Bio = "Yeni Bio", ImageUrl = "new.jpg" };

        // Act
        await _teacherService.UpdateAsync(teacher.Id, updateDto, "wwwroot");
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _context.Teachers.FindAsync(teacher.Id);
        Assert.Equal("Yeni Ad", updated!.Name);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Teacher_From_Db()
    {
        // Arrange
        var teacher = new Teacher { Name = "Silinəcək", Bio = "Temp Bio", ImageUrl = "temp.jpg" };
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        // Act
        await _teacherService.DeleteAsync(teacher.Id, "wwwroot");
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Teachers.FindAsync(teacher.Id);
        Assert.Null(result);
    }
}