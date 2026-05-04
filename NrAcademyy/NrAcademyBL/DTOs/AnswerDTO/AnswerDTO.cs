using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AnswerDTO
{
  
        public class AnswerCreateDto
        {
            [Required,MinLength(3),MaxLength(256)]
            public string Text { get; set; }=string.Empty;
            public bool IsCorrect { get; set; }
            [Required]
            public int QuestionId { get; set; }
        }
    }

