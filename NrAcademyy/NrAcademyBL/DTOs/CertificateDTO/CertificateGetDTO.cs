namespace NrAcademyBL.DTOs.CertificateDTO;

public class CertificateGetDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public string CertificateUrl { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public string CertificateType { get; set; }
    public int Score { get; set; }
    public string TestTitle { get; set; }
}
