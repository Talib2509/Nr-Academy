namespace NrAcademyBL.DTOs.TestResultDTO;

public class TestResultItemDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int TestId { get; set; }
    public int AppUserId { get; set; }
    public string UserFullName { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool IsWinner { get; set; }
}