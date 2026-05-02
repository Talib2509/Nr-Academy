using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.QuestionDTO;

public partial class QuestionDTO
{
    public class QuestionCreateDto
    {
        [Required,MinLength(3),MaxLength(256)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public int TestId { get; set; }
        [Required,MaxLength(256),MinLength(3)]
        public string QuestionType { get; set; } = string.Empty;
    }

}
