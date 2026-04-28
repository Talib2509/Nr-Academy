using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities.Identity;
using System.Text;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class UserServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly UserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly Mock<IWebHostEnvironment> _webHostMock;

    public UserServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. Identity Setup
        var userStore = new UserStore<AppUser, IdentityRole<int>, DbContext, int>(_dbContext);
        _userManager = new UserManager<AppUser>(userStore, null, new PasswordHasher<AppUser>(),
            null, null, null, null, null, null);

        // 2. WebHostEnvironment Mock (Fayl yükləmə yolu üçün)
        _webHostMock = new Mock<IWebHostEnvironment>();
        _webHostMock.Setup(m => m.WebRootPath).Returns(Path.GetTempPath()); // Test fayllarını müvəqqəti qovluğa atır

        _userService = new UserService(_userManager, _webHostMock.Object);
    }

    [Fact]
    public async Task GetUserByIdAsync_When_User_Exists_Should_Return_UserDto()
    {
        // Arrange
        var user = new AppUser { UserName = "mirtalib", Email = "m@nracademy.az" };
        await _userManager.CreateAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.UserName, result.UserName);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_When_Not_Exists_Should_Throw_Exception()
    {
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByIdAsync(9999));
    }

    [Fact]
    public async Task UploadProfileImageAsync_Should_Update_User_And_Save_File()
    {
        // Arrange
        var user = new AppUser { UserName = "imageuser", Email = "i@nracademy.az" };
        await _userManager.CreateAsync(user);

        // Saxta fayl yaradırıq (IFormFile mock)
        var content = "fake image content";
        var fileName = "test.jpg";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream target, CancellationToken ct) => stream.CopyToAsync(target, ct));

        // Act
        var resultUrl = await _userService.UploadProfileImageAsync(user.Id, fileMock.Object);

        // Assert
        var updatedUser = await _userManager.FindByIdAsync(user.Id.ToString());
        Assert.Contains("/uploads/profiles/", resultUrl);
        Assert.Equal(resultUrl, updatedUser!.ProfileImageUrl);

        // Faylın həqiqətən yaradıldığını yoxlayaq
        var fullPath = Path.Combine(_webHostMock.Object.WebRootPath, resultUrl.TrimStart('/'));
        Assert.True(File.Exists(fullPath));

        // Test bitəndə təmizlik (isteğe bağlı)
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}