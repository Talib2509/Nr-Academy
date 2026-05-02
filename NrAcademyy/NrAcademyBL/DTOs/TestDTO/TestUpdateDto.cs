using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.TestDTO
{
    public partial class TestDTO
    {
        public class TestUpdateDto
        {
            public int Id { get; set; }
            [Required, MinLength(3), MaxLength(256)]
            public string Title { get; set; }= string.Empty;
            [Required, MinLength(3), MaxLength(256)]
            public string Description { get; set; } = string.Empty;
            [Required]
            public int CourseId { get; set; }
        }
    }
}
