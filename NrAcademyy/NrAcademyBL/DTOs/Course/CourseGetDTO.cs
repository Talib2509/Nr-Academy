using NrAcademyBL.DTOs.Course;
using NrAcademyCORE.Enums;

namespace NrAcademyBL.DTOs.CourseDTOs
{
    public class CourseGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public Levels Level { get; set; }
        public int Duration { get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public TeacherInCourseDTO Teacher { get; set; }
    }
}