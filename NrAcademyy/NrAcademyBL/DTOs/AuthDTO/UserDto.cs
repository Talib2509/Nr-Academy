using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(256)]
        public string UserName { get; set; } = null!;
        [Required,EmailAddress]
        public string Email { get; set; } = null!;
        [Required,Url]
        public string? ProfileImageUrl { get; set; }
    }
}
