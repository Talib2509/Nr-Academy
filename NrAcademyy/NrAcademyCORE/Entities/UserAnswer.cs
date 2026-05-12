
using NrAcademyCORE.Entities;
using NrAcademyCORE.Entities.Common;

public class UserAnswer : BaseEntity
{
    public int TestResultId { get; set; }
    public virtual TestResult TestResult { get; set; } = null!;

    public int QuestionId { get; set; }
    public virtual Question Question { get; set; } = null!;

    public int SelectedAnswerId { get; set; }
    public bool IsCorrect { get; set; } // O an üçün düz idi ya səhv
}