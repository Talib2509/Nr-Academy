using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.TestDTO
{
  
        public class TestCreateDto
        {
            [Required, MinLength(3), MaxLength(256)]
            public string Title { get; set; }=string.Empty;
            [Required, MinLength(3), MaxLength(256)]
            public string Description { get; set; }=string.Empty;
            [Required]
            public int CourseId { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
    }
    }

