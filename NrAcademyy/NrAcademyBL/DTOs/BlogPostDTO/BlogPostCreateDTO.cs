
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using Microsoft.AspNetCore.Http;


namespace NrAcademyBL.DTOs.BlogPostDTO
{

    [Required,MaxLength(500),MinLength(3)]
    public string Title { get; set; } = string.Empty;
    [Required,MaxLength(500),MinLength(3)]
    public string Content { get; set; } = string.Empty;
    [Required]
    public int CategoryId { get; set; }
}

    public class BlogPostCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
    }


}
