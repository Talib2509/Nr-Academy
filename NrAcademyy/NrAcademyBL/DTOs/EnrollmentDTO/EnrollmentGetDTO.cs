using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.DTOs.EnrollmentDTO;

public class EnrollmentGetDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; }
}
