using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.EnrollmentDTO
{
    public class EnrollmentCreateDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CourseId { get; set; }
    }
}
