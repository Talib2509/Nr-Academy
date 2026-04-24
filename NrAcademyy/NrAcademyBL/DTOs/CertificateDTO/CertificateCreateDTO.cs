using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.CertificateDTO;

public class CertificateCreateDTO
{
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CertificateUrl { get; set; } = string.Empty;
    }

