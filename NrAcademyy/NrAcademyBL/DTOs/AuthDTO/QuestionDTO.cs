using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO
{
    public class QuestionDTO
    {
        public class QuestionCreateDto
        {
            public string Text { get; set; }
            public int TestId { get; set; }
            public string QuestionType { get; set; }
        }

        public class QuestionUpdateDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int TestId { get; set; }
            public string QuestionType { get; set; }
        }

        public class QuestionItemDto
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public int TestId { get; set; }
            public string QuestionType { get; set; }
        }

    }
}
