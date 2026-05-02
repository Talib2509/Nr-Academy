using NrAcademyBL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.Teacher;

public class TeacherNotFoundException:BaseException
{
    public TeacherNotFoundException(string message = "Müəllim tapılmadı!") : base(404, message)
    {
    }

    public TeacherNotFoundException(int id) : base(404, $"ID-si {id} olan müəllim tapılmadı.")
    {
    }
}
