
﻿using System.ComponentModel.DataAnnotations;
﻿using Microsoft.AspNetCore.Http;


namespace NrAcademyBL.DTOs.BlogPostDTO;

public class BlogPostUpdateDTO
{
    public int Id { get; set; }
    [Required, MaxLength(500), MinLength(3)]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(100), MinLength(3)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }
    public IFormFile? ImageFile { get; set; }
}