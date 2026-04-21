using NrAcademyBL.DTOs;
using NrAcademyBL.DTOs.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDto> LoginAsync(LoginDTO dto);

    }
}
