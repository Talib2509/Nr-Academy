using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        var userStore = new UserStore<AppUser, IdentityRole<int>, DbContext, int>(_context);

        // UserManager-in işləməsi üçün lazımi opsiyalar
        var options = new Mock<IOptions<IdentityOptions>>();
        var idOptions = new IdentityOptions();
        idOptions.Lockout.AllowedForNewUsers = false;
        options.Setup(o => o.Value).Returns(idOptions);

        _userManager = new UserManager<AppUser>(userStore, options.Object, new PasswordHasher<AppUser>(),
            null, null, null, null, null, null);

        // 2. Mocking IWebHostEnvironment
        _webHostMock = new Mock<IWebHostEnvironment>();
        _webHostMock.Setup(m => m.WebRootPath).Returns(Path.GetTempPath());

        // SERVİSİ DÜZGÜN PARAMETRLƏ BAŞLADIN (Yalnız UserManager)
        _userService = new UserService(_userManager);
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
        // 1. Exception yerinə NrAcademyBL.Exceptions.User.UserException yazın
        var ex = await Assert.ThrowsAsync<NrAcademyBL.Exceptions.User.UserException>(
            () => _userService.GetUserByIdAsync(9999)
        );

        // 2. Mesajın tam olaraq eyni olduğunu yoxlayın (Nöqtəsinə qədər)
        Assert.Equal("ID: 9999 olan istifadəçi tapılmadı.", ex.Message);
    }

    [Fact]
 
    public async Task UploadProfileImageAsync_Should_Update_User_And_Save_File()
    {
        // Arrange
        var user = new AppUser { UserName = "imageuser", Email = "i@nracademy.az" };
        await _userManager.CreateAsync(user);

        // IFormFile Mock (Sizin yazdığınız hissə doğrudur, sadəcə ContentType əlavə edin)
        var content = "fake image content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.ContentType).Returns("image/jpeg"); // IsValidType yoxlaması üçün mütləqdir

        // ACT - İkinci parametri (rootPath) mütləq ötürün
        var resultUrl = await _userService.UploadProfileImageAsync(user.Id, fileMock.Object, _webHostMock.Object.WebRootPath);

        // ASSERT
        var updatedUser = await _userManager.FindByIdAsync(user.Id.ToString());
        Assert.NotNull(updatedUser!.ProfileImageUrl);
        Assert.Contains("/uploads/profiles/", resultUrl);
    }
}