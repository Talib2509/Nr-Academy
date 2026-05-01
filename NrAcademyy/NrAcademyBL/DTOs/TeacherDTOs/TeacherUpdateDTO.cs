using Microsoft.AspNetCore.Http;

namespace NrAcademyBL.DTOs.TeacherDTOs
{
    public class TeacherUpdateDTO
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int Experience { get; set; }
    }
}