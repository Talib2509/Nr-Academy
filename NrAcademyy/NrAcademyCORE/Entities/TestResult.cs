using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class TestResult : BaseEntity
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public DateTime ComletedAt { get; set; } = DateTime.Now;
    }
}
