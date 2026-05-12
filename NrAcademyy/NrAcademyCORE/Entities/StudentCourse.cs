using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NrAcademyCORE.Entities
{
    public class StudentCourse : BaseEntity
    {

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Student { get; set; }

        // Kursun Id-si
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = false;
        public int ProgressPercentage { get; set; }
    }
}