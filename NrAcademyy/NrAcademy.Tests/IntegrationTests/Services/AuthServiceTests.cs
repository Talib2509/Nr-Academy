using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.DTOs;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Extensions;
using NrAcademyBL.Services.Abstract;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Enums;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class AuthServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly AuthService _authService;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public AuthServiceTests(TestDbContextFactory factory) : base(factory)
    {
        _jwtServiceMock = new Mock<IJwtService>();
        _emailServiceMock = new Mock<IEmailService>();

        // PasswordHasher əlavə etmək Login testlərindəki null xətalarını aradan qaldıracaq
        var passwordHasher = new PasswordHasher<AppUser>();
        var userStore = new UserStore<AppUser, IdentityRole<int>, DbContext, int>(_dbContext);

        _userManager = new UserManager<AppUser>(
        new UserStore<AppUser, AppRole, DbContext, int>(_dbContext),
        null,
        new PasswordHasher<AppUser>(), // Parol yoxlanışı üçün lazımdır
        null, null, null, null, null, null);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(AuthService).Assembly);
        });
        _mapper = mapperConfig.CreateMapper();

        _authService = new AuthService(
            _userManager,
            _mapper,
            _jwtServiceMock.Object,
            _emailServiceMock.Object
        );

        _emailServiceMock.Setup(x => x.SendVerificationEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                         .ReturnsAsync(true);
    }

    [Fact]
    public async Task RegisterAsync_Should_Create_User_And_Send_Verification_Email()
    {
        // Arrange
        var dto = new RegisterDTO
        {
            Email = "teststudent@nracademy.az",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "Student",
            Role = Roles.Student
        };

        // Act
        await _authService.RegisterAsync(dto);

        // Assert
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        Assert.NotNull(user);
        Assert.False(user.IsEmailVerified);
        Assert.NotNull(user.EmailVerificationCode);
        Assert.NotNull(user.EmailVerificationCodeExpiry);

        _emailServiceMock.Verify(x => x.SendVerificationEmailAsync(dto.Email, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_With_Valid_Credentials_And_Verified_Email_Should_Return_Tokens()
    {
        // Arrange
        var user = await CreateVerifiedTestUserAsync(Roles.Student);

        _jwtServiceMock.Setup(x => x.GenerateTokensAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                      .ReturnsAsync(new AuthResponseDto
                      {
                          AccessToken = "fake-jwt-token",
                          RefreshToken = "fake-refresh-token"
                      });

        var loginDto = new LoginDTO
        {
            Email = user.Email!,
            Password = "Test123!"
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
    }

    [Fact]
    public async Task LoginAsync_With_Unverified_Email_Should_Throw_Exception()
    {
        // Arrange
        var user = await CreateTestUserAsync(Roles.Student); // verified = false

        var loginDto = new LoginDTO
        {
            Email = user.Email!,
            Password = "Test123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
            await _authService.LoginAsync(loginDto));
    }

    // ===================== Helper Methods =====================

    private async Task<AppUser> CreateTestUserAsync(Roles role)
    {
        var user = new AppUser
        {
            Email = $"test{role.ToString().ToLower()}@nracademy.az",
            UserName = $"test{role.ToString().ToLower()}@nracademy.az",
         
            IsEmailVerified = false,
            EmailVerificationCode = "123456",
            EmailVerificationCodeExpiry = DateTime.UtcNow.AddMinutes(10)
        };

        await _userManager.CreateAsync(user, "Test123!");
        await _userManager.AddToRoleAsync(user, role.GetRole());
        return user;
    }

    private async Task<AppUser> CreateVerifiedTestUserAsync(Roles role)
    {
        var user = await CreateTestUserAsync(role);
        user.IsEmailVerified = true;
        user.EmailVerificationCode = null;
        user.EmailVerificationCodeExpiry = null;
        await _userManager.UpdateAsync(user);
        return user;
    }
}