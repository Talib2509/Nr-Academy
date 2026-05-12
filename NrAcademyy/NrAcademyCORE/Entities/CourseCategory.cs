using NrAcademyCORE.Entities.Common;
using System.Collections.Generic;

namespace NrAcademyCORE.Entities
{
    public class CourseCategory : BaseEntity
    {
        public string Name { get; set; } 
        public string? Description { get; set; }
        public string? IconUrl { get; set; } 
        
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}