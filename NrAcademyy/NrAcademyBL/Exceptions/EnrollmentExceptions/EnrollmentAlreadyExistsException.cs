using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Abstractions;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.EnrollmentExceptions;

public class EnrollmentAlreadyExistsException : BaseException
{
    
    public EnrollmentAlreadyExistsException(string message = "Tələbə artıq bu kursa qeydiyyatdan keçib!")
        : base(message, StatusCodes.Status400BadRequest, "ENROLLMENT_ALREADY_EXISTS")
    {
    }
}