namespace NrAcademyBL.DTOs.TestResultDTO;

public class TestResultItemDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int TestId { get; set; }
    public int AppUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
