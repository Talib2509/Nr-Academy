using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class DailyWinner : BaseEntity
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public DateTime WinDate { get; set; } 
        public int FinalScore { get; set; }
        public int CompletionTime { get; set; } 
    }
}
