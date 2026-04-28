using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO
{
    public class VerifyEmailDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
