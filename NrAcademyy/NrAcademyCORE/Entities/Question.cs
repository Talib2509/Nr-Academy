using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class Question : BaseEntity
    {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int TestId { get; set; }
      
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();


    }
}
