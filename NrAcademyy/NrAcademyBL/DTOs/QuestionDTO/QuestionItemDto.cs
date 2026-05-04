namespace NrAcademyBL.DTOs.QuestionDTO
{
    
        public class QuestionItemDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public int TestId { get; set; }
            public string QuestionType { get; set; } = string.Empty;
        }

    }

