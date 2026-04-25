using NrAcademyBL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailDto email);
        Task<bool> SendVerificationEmailAsync(string email, string verificationCode);
    }
}
