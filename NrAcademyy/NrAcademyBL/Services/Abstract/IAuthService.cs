using NrAcademyBL.DTOs;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.DTOs.ForgotPassword;
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
        Task ForgotPasswordAsync(ForgotPasswordDTO dto);
        Task VerifyResetCodeAsync(VerifyResetCodeDTO dto);
        Task ResetPasswordAsync(ResetPasswordDTO dto);

    }
}
