using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.BlogCategoryDTO;

public class BlogCategoryCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
