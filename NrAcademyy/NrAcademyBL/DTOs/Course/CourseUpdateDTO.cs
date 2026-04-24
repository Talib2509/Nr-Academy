using NrAcademyCORE.Enums;

namespace NrAcademyBL.DTOs.CourseDTOs
{
    public class CourseUpdateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public Levels Level { get; set; }
        public int Duration { get; set; }
        public int TeacherId { get; set; }
    }
}