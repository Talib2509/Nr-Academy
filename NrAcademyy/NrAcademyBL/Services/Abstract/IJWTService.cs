using NrAcademyBL.DTOs;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{

    public interface IJwtService
    {
        Task<AuthResponseDto> GenerateTokensAsync(AppUser user, string ipAddress);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task RevokeTokenAsync(string refreshToken, string ipAddress);
    }
}
