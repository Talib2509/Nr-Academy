using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public BlogCategory? Category { get; set; }
    }
}
