using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.BlogCategoryDTO;

public class BlogCategoryCreateDTO
{
    [Required,MaxLength(100),MinLength(3)]
    public string Name { get; set; } = string.Empty;
    [Required,MaxLength(500),MinLength(3)]
    public string Slug { get; set; } = string.Empty;
}
