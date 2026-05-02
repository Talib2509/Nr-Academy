using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.TestResultDTO;

public class TestResultUpdateDto
{
    public int Id { get; set; }
    [Required,Range(0,100)]
    public int Score { get; set; }
    [Required]
    public int TestId { get; set; }
    [Required]
    public int AppUserId { get; set; }
}
