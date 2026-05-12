using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyCORE.Entities;

public class DailyLeaderboard
{
   
    public int StudentId { get; set; }

  
    public int TestId { get; set; }

    public DateTime Date { get; set; }
}
