using System.ComponentModel.DataAnnotations;

public class EnrollmentUpdateDTO
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int CourseId { get; set; }
}