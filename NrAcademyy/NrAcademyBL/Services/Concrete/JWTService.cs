using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NrAcademyBL.Configuration;
using NrAcademyBL.DTOs;
using NrAcademyBL.Exceptions.Auth; 
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NrAcademyBL.Services.Concrete
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly AppDbContext _context;

        public JwtService(
            UserManager<AppUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshRepo,
            AppDbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _refreshRepo = refreshRepo;
            _context = context;
        }

        public async Task<AuthResponseDto> GenerateTokensAsync(AppUser user, string ip)
        {
            try
            {
                if (user == null)
                    throw JwtException.UserNotFoundForToken("Bilinməyən");

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenMinutes),
                    signingCredentials: creds
                );

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays),
                    AppUserId = user.Id,
                    CreatedByIp = ip
                };

                await _refreshRepo.AddAsync(refreshToken);
                await _refreshRepo.SaveAsync();

                return new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                };
            }
            catch (Exception ex) when (!(ex is JwtException))
            {
                throw JwtException.TokenGenerationFailed(ex.Message);
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token, string ip)
        {
            if (string.IsNullOrEmpty(token))
                throw JwtException.InvalidRefreshToken();

            var existing = await _refreshRepo.GetByTokenAsync(token);

            // Token tapılmadısa və ya ləğv edilibsə
            if (existing == null || existing.IsRevoked)
                throw JwtException.InvalidRefreshToken();

            // Vaxtı bitibsə
            if (existing.IsExpired)
                throw JwtException.RefreshTokenExpired();

            // Token rotation (Köhnəni ləğv et)
            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;

            var user = existing.User;
            if (user == null)
                throw JwtException.UserNotFoundForToken(existing.AppUserId.ToString());

            var newTokens = await GenerateTokensAsync(user, ip);

            existing.ReplacedByToken = newTokens.RefreshToken;
            await _refreshRepo.SaveAsync();

            return newTokens;
        }

        public async Task RevokeTokenAsync(string token, string ip)
        {
            var existing = await _refreshRepo.GetByTokenAsync(token);

            // Revoke edərkən token tapılmazsa səssizcə qayıtmaq olar, 
            // amma daha sərt kontrol üçün exception da fırlada bilərsən.
            if (existing == null) 
                throw JwtException.InvalidRefreshToken();

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;

            await _refreshRepo.SaveAsync();
        }
    }
}