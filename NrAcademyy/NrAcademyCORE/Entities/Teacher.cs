using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class Teacher : BaseEntity
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public int Experience { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
