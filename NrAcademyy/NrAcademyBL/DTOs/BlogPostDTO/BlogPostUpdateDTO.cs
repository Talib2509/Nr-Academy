namespace NrAcademyBL.DTOs.BlogPostDTO;

public class BlogPostUpdateDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }

}
