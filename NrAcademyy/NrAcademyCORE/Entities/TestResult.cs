using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Entities.Identity;
using System;

namespace NrAcademyCORE.Entities
{
    public class TestResult : BaseEntity
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public int Score { get; set; }
        public int DurationInSeconds { get; set; } 
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.Now;
        public bool IsWinner { get; set; } = false;
        public virtual AppUser User { get; set; }
    }
}