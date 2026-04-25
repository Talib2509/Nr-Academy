using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.AuthDTO;

public class TestResultCreateDto
{
    public int Score { get; set; }
    public int TestId { get; set; }
    public int AppUserId { get; set; }
}

public class TestResultUpdateDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int TestId { get; set; }
    public int AppUserId { get; set; }
}

public class TestResultItemDto
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int TestId { get; set; }
    public int AppUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
