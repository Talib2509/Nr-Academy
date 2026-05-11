using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.TestiomonialDTO;

public class TestimonialCreateDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int TestId { get; set; }

    [Required,MinLength(5),MaxLength(1000)]
    public string ReviewText { get; set; } = null!;

    [Required,Range(1,5)]
    public int Rating { get; set; }
}
