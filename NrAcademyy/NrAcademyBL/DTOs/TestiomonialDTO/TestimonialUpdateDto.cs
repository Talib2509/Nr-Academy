using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.TestiomonialDTO;

public class TestimonialUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required,MinLength(5),MaxLength(100)]
    public string ReviewText { get; set; } = null!;
    [Required,Range(1, 5)]
    public int Rating { get; set; }
}