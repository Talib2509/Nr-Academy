using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.CertificateDTO;

public class CertificateCreateDTO
{
    [Required]
    public int UserId { get; set; }

    public string UserFullName { get; set; } = string.Empty;

    [Required]
    public int CourseId { get; set; }

    public string CertificateUrl { get; set; } = string.Empty;
    public string CertificateType { get; set; } = string.Empty;
    public int Score { get; set; }
    public string TestTitle { get; set; } = string.Empty;
}
