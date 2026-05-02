using NrAcademyCORE.Entities.Common;

namespace NrAcademyCORE.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public BlogCategory Category { get; set; }
    }
}