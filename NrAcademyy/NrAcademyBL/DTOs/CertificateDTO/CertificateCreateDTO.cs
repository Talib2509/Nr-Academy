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
    [Required]
    public int CourseId { get; set; }
    [Required,Url]
    public string CertificateUrl { get; set; } = string.Empty;
}
