using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Entities.Identity;
using NrAcademyCORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public Levels Level { get; set; }
        public int Duration { get; set; }
        public int TeacherId { get; set; } 
        public AppUser Teacher { get; set; }
       
       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}