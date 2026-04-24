using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.BlogPostDTO;

public class BlogPostCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public int CategoryId { get; set; }
}
