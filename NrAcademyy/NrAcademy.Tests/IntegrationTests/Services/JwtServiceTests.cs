using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NrAcademy.Tests.IntegrationTests;
using NrAcademyBL.Configuration;
using NrAcademyBL.Services.Concrete;
using NrAcademyCORE.Entities.Identity;
using NrAcademyDAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace NrAcademy.Tests.IntegrationTests.Services;

public class JwtServiceTests : BaseIntegrationTest, IClassFixture<TestDbContextFactory>
{
    private readonly JwtService _jwtService;
    private readonly UserManager<AppUser> _userManager;
    private readonly RefreshTokenRepository _refreshRepo;

    public JwtServiceTests(TestDbContextFactory factory) : base(factory)
    {
        // 1. JwtSettings ayarlarını simulyasiya edirik
        var jwtSettings = new JwtSettings
        {
            Secret = "Bu_Cox_Gizli_Ve_Uzun_Bir_Key_Olmalidir_123456",
            Issuer = "NrAcademy",
            Audience = "NrAcademyUsers",
            AccessTokenMinutes = 15,
            RefreshTokenDays = 7
        };
        var options = Options.Create(jwtSettings);

        // 2. Repository və UserManager quraşdırılması
        _refreshRepo = new RefreshTokenRepository(_dbContext);

        var userStore = new UserStore<AppUser, AppRole, DbContext, int>(_dbContext);
        _userManager = new UserManager<AppUser>(userStore, null, new PasswordHasher<AppUser>(),
            null, null, null, null, null, null);

        _jwtService = new JwtService(_userManager, options, _refreshRepo, _dbContext);
    }

    [Fact]
    public async Task GenerateTokensAsync_Should_Create_Valid_Jwt_And_Save_RefreshToken()
    {
        // Arrange
        var user = new AppUser { UserName = "mirtalib", Email = "m@nracademy.az" };
        await _userManager.CreateAsync(user, "Test123!");

        // Act
        var result = await _jwtService.GenerateTokensAsync(user, "127.0.0.1");

        // Assert
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);

        // Tokenin oxunaqlı olub olmadığını yoxlayaq
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result.AccessToken);
        Assert.Equal("mirtalib", token.Claims.First(c => c.Type == "unique_name").Value);

        // Refresh token bazaya yazılıbmı?
        var dbToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == result.RefreshToken);
        Assert.NotNull(dbToken);
        Assert.Equal(user.Id, dbToken.AppUserId);
    }

    [Fact]
    public async Task RefreshTokenAsync_Should_Revoke_Old_And_Generate_New_Token()
    {
        // Arrange
        var user = new AppUser { UserName = "testuser", Email = "t@nracademy.az" };
        await _userManager.CreateAsync(user, "Test123!");

        var oldTokenResponse = await _jwtService.GenerateTokensAsync(user, "127.0.0.1");

        // Act
        var result = await _jwtService.RefreshTokenAsync(oldTokenResponse.RefreshToken, "127.0.0.1");

        // Assert
        Assert.NotEqual(oldTokenResponse.RefreshToken, result.RefreshToken);

        // Köhnə token ləğv edilibmi? (IsRevoked yoxlanışı)
        var revokedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == oldTokenResponse.RefreshToken);
        Assert.True(revokedToken!.IsRevoked);
        Assert.Equal(result.RefreshToken, revokedToken.ReplacedByToken);
    }

    [Fact]
    public async Task RevokeTokenAsync_Should_Set_IsRevoked_To_True()
    {
        // Arrange
        var user = new AppUser { UserName = "revokeuser", Email = "r@nracademy.az" };
        await _userManager.CreateAsync(user, "Test123!");
        var tokenRes = await _jwtService.GenerateTokensAsync(user, "127.0.0.1");

        // Act
        await _jwtService.RevokeTokenAsync(tokenRes.RefreshToken, "127.0.0.1");

        // Assert
        var dbToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRes.RefreshToken);
        Assert.True(dbToken!.IsRevoked);
        Assert.NotNull(dbToken.RevokedAt);
    }
}