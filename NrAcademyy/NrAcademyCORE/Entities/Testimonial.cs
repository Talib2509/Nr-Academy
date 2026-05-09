using NrAcademyCORE.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities;

public class Testimonial:BaseEntity
{
    public int UserId { get; set; }
    public int TestId { get; set; }
    public string ReviewText { get; set; } = null!;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

