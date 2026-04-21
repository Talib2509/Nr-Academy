using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities
{
    public class BlogCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
