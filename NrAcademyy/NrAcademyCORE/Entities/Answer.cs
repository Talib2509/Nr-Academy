using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
