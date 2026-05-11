using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.TestResultDTO;

public class TestResultCreateDto
{
    [Required,Range(0,100)]
    public int Score { get; set; }
    [Required]
    public int TestId { get; set; }
    [Required]
    public int AppUserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
}
