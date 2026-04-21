using Microsoft.AspNetCore.Identity;

using Microsoft.IdentityModel.Tokens;
using NrAcademyBL.DTOs;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using NrAcademyBL.Exceptions.AuthException;
using Microsoft.Extensions.Configuration;
namespace NrAcademyBL.Services.Concrete
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly AppDbContext _context;

        public JwtService(UserManager<AppUser> userManager,
                          IConfiguration config,
                          IRefreshTokenRepository refreshRepo, AppDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _refreshRepo = refreshRepo;
            _context = context;
        }

        public async Task<AuthResponseDto> GenerateTokensAsync(AppUser user, string ip)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Secret"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:AccessTokenMinutes"])
                ),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Expires = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenDays"])
                ),
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

        public async Task<AuthResponseDto> RefreshTokenAsync(string token, string ip)
        {
            var existing = await _refreshRepo.GetByTokenAsync(token);
            if (string.IsNullOrEmpty(token)) return null;


            //return await _context.RefreshTokens
            //    .Include(x => x.User) 
            //    .FirstOrDefaultAsync(x => x.Token == token);

            if (existing == null || !existing.IsActive)
                throw new Exception("Invalid token");

            // rotation
            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;

            var user = existing.User;

            var newTokens = await GenerateTokensAsync(user, ip);

            existing.ReplacedByToken = newTokens.RefreshToken;

            await _refreshRepo.SaveAsync();


            return newTokens;
        }

        public async Task RevokeTokenAsync(string token, string ip)
        {
            var existing = await _refreshRepo.GetByTokenAsync(token);

            if (existing == null) return;

            existing.IsRevoked = true;
            existing.RevokedAt = DateTime.UtcNow;

            await _refreshRepo.SaveAsync();
        }
    }
}
