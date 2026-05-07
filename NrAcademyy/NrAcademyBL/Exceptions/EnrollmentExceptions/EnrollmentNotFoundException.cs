using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.EnrollmentExceptions;

public class EnrollmentNotFoundException : BaseException
{
    // Ardıcıllıq: base(message, statusCode, errorCode)
    public EnrollmentNotFoundException(string message = "Qeydiyyat tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "ENROLLMENT_NOT_FOUND")
    {
    }
}