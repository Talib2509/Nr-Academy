namespace NrAcademyBL.DTOs.AnswerDTO
{
    public partial class AnswerDTO
    {
        public class AnswerItemDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public bool IsCorrect { get; set; }
            public int QuestionId { get; set; }
        }
    }
}
