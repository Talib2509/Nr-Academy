using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.BlogCategoryDTO;

public class BlogCategoryUpdateDTO
{
    public int Id { get; set; }
    [Required, MaxLength(100), MinLength(3)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(500), MinLength(3)]
    public string Slug { get; set; } = string.Empty;
}
