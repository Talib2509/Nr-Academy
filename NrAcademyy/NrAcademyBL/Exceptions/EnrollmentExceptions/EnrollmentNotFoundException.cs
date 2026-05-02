using NrAcademyBL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.EnrollmentExceptions;

public class EnrollmentNotFoundException:BaseException
{
    public EnrollmentNotFoundException(string message = "Qeydiyyat tapılmadı!") : base(404, message)
    {
    }
}
