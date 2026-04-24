namespace NrAcademyBL.DTOs.CertificateDTO;

public class CertificateUpdateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string CertificateUrl { get; set; } = string.Empty;
    }

