using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO
{
    public class AnswerDTO
    {
        public class AnswerCreateDto
        {
            public string Text { get; set; }
            public bool IsCorrect { get; set; }
            public int QuestionId { get; set; }
        }

        public class AnswerUpdateDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsCorrect { get; set; }
            public int QuestionId { get; set; }
        }

        public class AnswerItemDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsCorrect { get; set; }
            public int QuestionId { get; set; }
        }
    }
}
