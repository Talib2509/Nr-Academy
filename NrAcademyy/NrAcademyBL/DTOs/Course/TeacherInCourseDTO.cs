using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.Course
{
    public class TeacherInCourseDTO
    {
        public int Id { get; set; }
        [Required,MaxLength(100),MinLength(6)]
        public string FullName { get; set; }=string.Empty;
    }
}

