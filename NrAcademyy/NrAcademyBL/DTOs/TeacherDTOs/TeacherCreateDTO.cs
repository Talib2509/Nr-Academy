
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;


namespace NrAcademyBL.DTOs.TeacherDTOs
{
    public class TeacherCreateDTO
    {

        [Required,MinLength(6),MaxLength(100)]
        public string Name { get; set; }=string.Empty;
        [Required,MinLength(10),MaxLength(100)]
        public string Bio { get; set; } = string.Empty;
 
        [Required,Range(0,50)]

    
        public IFormFile ImageFile { get; set; }

        public int Experience { get; set; }
    }
}