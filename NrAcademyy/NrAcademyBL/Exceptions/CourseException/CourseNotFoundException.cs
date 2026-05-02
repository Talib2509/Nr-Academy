using NrAcademyBL.Abstractions;
using NrAcademyBL.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.CourseException;

public class CourseNotFoundException : BaseException
{
    public CourseNotFoundException(string message = "Kurs tapılmadı!") : base(404, message)
    {
    }

    public CourseNotFoundException(int id) : base(404, $"ID-si {id} olan kurs sistemdə mövcud deyil.")
    {
    }

}