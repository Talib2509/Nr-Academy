using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.QuestionDTO
{
    public partial class QuestionDTO
    {
        public class QuestionUpdateDto
        {
            public int Id { get; set; }
            [Required, MinLength(3), MaxLength(256)]
            public string Text { get; set; }=string.Empty;
            [Required]
            public int TestId { get; set; }
            [Required, MinLength(3), MaxLength(256)]
            public string QuestionType { get; set; } = string.Empty;
        }

    }
}
