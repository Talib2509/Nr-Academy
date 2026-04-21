using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class Test : BaseEntity
    {
        public string Title { get; set; }
        public int CourseId { get; set; }

        public string? Duration { get; set; }
        public int PassingScore { get; set; }
    }
}
