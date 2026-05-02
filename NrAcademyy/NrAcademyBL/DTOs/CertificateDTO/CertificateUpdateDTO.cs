using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.CertificateDTO;

public class CertificateUpdateDTO
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required,Url]
    public string CertificateUrl { get; set; } = string.Empty;
}