using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs.CourseDTOs;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Enums;
using NrAcademyDAL.Repositories; // Real repository-ni istifadə edirik
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class CourseServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly CourseService _courseService;
    private readonly IMapper _mapper;
    private readonly CourseRepository _courseRepo;

    public CourseServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. AutoMapper quraşdırılması
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(CourseService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        // 2. Real Repository (çünki inteqrasiya testi edirik)
        _courseRepo = new CourseRepository(_dbContext);

        // 3. Servisin yaradılması
        _courseService = new CourseService(_courseRepo, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Course_To_Database()
    {
        // Arrange: Müəllim mütləq lazımdır, çünki Course TeacherId tələb edir
        var teacher = new Teacher { Name = "Mirtalib", Bio = "Senior Dev", ImageUrl = "m.jpg" };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        var dto = new CourseCreateDTO
        {
            Title = "Fullstack .NET Development",
            Description = "Learn C# and React", // Required
            Price = 500,
            ImageUrl = "course.jpg",             // Required
            Level = Levels.Beginner,             // Required (Enum-dan asılı olaraq)
            Duration = 6,                        // Required
            TeacherId = teacher.Id               // Required: Yuxarıda yaratdığımız müəllimin Id-si
        };

        // Act
        await _courseService.CreateAsync(dto);
        await _dbContext.SaveChangesAsync();

        // Assert
        var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Title == dto.Title);
        Assert.NotNull(course);
        Assert.Equal(teacher.Id, course.TeacherId);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Courses_With_Teacher_Information()
    {
        // Arrange
        var teacher = new Teacher { Name = "Mirtalib", Bio = "Bio", ImageUrl = "t.jpg" };
        var course = new Course
        {
            Title = "Backend Masterclass",
            Description = "Advanced backend course",
            ImageUrl = "masterclass.jpg",
            Level = Levels.Intermediate,
            Duration = 12,
            Teacher = teacher, // Müəllim obyekti avtomatik TeacherId-ni dolduracaq
            Price = 1000
        };

        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _courseService.GetAllAsync();

        // Assert
        Assert.NotEmpty(result);
        var firstCourse = result.First();
        // DTO-da TeacherName mapi konfiqurasiya edilibsə bu Assert keçəcək
        Assert.Contains("Mirtalib", firstCourse.TeacherName);
    }

    [Fact]
    public async Task GetByIdAsync_When_Course_Exists_Should_Return_Course()
    {
        // Arrange
        var teacher = new Teacher { Name = "T", Bio = "B", ImageUrl = "i.jpg" };
        var course = new Course
        {
            Title = "Entity Framework Core",
            Price = 200,
            Description = "EF Core course",
            ImageUrl = "ef.jpg",
            Level = Levels.Advanced,
            Duration = 5,
            Teacher = teacher
        };
        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _courseService.GetByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Entity Framework Core", result.Title);
    }

    [Fact]
    public async Task UpdateAsync_Should_Change_Course_Details()
    {
        // Arrange
        var teacher = new Teacher { Name = "T", Bio = "B", ImageUrl = "i.jpg" };
        var course = new Course
        {
            Title = "Old Title",
            Price = 100,
            Description = "Old Desc",
            ImageUrl = "old.jpg",
            Level = Levels.Beginner,
            Duration = 1,
            Teacher = teacher
        };
        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        var updateDto = new CourseUpdateDTO
        {
            Title = "New Updated Title",
            Price = 150,
            Description = "New Desc",
            ImageUrl = "new.jpg"
        };

        // Act
        await _courseService.UpdateAsync(course.Id, updateDto);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedCourse = await _dbContext.Courses.FindAsync(course.Id);
        Assert.Equal("New Updated Title", updatedCourse!.Title);
        Assert.Equal(150, updatedCourse.Price);
    }



    [Fact]
    public async Task GetByIdAsync_When_Not_Exists_Should_Throw_Exception()
    {
        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _courseService.GetByIdAsync(999));
        Assert.Equal("Course not found", ex.Message);
    }



    [Fact]
    public async Task DeleteAsync_Should_Remove_Course_From_Db()
    {
        // Arrange
        var teacher = new Teacher { Name = "T", Bio = "B", ImageUrl = "i.jpg" };
        await _dbContext.Teachers.AddAsync(teacher);
        // Müəllimi yadda saxlayırıq ki, Id-si yaransın
        await _dbContext.SaveChangesAsync();

        var course = new Course
        {
            Title = "Delete Me",
            Price = 0,
            Description = "Silinəcək kursun təsviri", // VACİB
            ImageUrl = "delete.jpg",                // VACİB
            Level = Levels.Beginner,                // VACİB
            Duration = 1,                           // VACİB
            TeacherId = teacher.Id                  // VACİB
        };

        await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync(); // Burada xəta alırdınız, indi keçəcək

        // Act
        await _courseService.DeleteAsync(course.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        var deletedCourse = await _dbContext.Courses.FindAsync(course.Id);
        Assert.Null(deletedCourse);
    }
}