using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.BlogPostDTO;

public class BlogPostCreateDTO
{
    [Required,MaxLength(500),MinLength(3)]
    public string Title { get; set; } = string.Empty;
    [Required,MaxLength(500),MinLength(3)]
    public string Content { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
}
