using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.BlogCategoryDTO
{
    public class BlogCategoryGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; }    = string.Empty;
    }
}
