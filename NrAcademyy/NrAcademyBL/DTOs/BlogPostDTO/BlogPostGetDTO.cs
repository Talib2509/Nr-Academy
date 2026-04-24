namespace NrAcademyBL.DTOs.BlogPostDTO;

public class BlogPostGetDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;


    public DateTime CreatedAt { get; set; } 

    public string CategoryName { get; set; } = string.Empty;

}
