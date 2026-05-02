using NrAcademyCORE.Enums;
using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.CourseDTOs
{
    public class CourseCreateDTO
    {
        [Required,MinLength(3),MaxLength(100)]
        public string Title { get; set; }=string.Empty;
        [Required,MinLength(3),MaxLength(300)]
        public string Description { get; set; }=string.Empty;
        [Required,Range(0,10000)]
        public int Price { get; set; }
        [Required,Url]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]
        public Levels Level { get; set; }
        [Required,Range(0,1000)]
        public int Duration { get; set; }
        [Required]
        public int TeacherId { get; set; }
    }
}