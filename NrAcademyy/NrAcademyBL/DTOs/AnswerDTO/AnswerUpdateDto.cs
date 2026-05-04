using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.AnswerDTO
{

    public class AnswerUpdateDto
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(256)]
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        [Required]
        public int QuestionId { get; set; }
    }
}
