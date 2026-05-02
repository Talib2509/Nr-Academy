using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.ForgotPassword
{
    public class VerifyResetCodeDTO
    {
        [Required,EmailAddress]
        public string Email { get; set; }=string.Empty;
        [Required, MaxLength(256), MinLength(6),]
        public string Code { get; set; }=string.Empty;
    }
}
