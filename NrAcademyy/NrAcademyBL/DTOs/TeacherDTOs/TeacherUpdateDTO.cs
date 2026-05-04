using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NrAcademyBL.DTOs.TeacherDTOs
{
    public class TeacherUpdateDTO
    {
        [Required, MinLength(6), MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MinLength(10), MaxLength(100)]
        public string Bio { get; set; } = string.Empty;
        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;
        [Required, Range(0, 50)]


        public IFormFile ImageFile { get; set; }

        public int Experience { get; set; }
    }
}