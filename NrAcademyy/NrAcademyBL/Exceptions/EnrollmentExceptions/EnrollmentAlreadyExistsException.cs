using NrAcademyBL.Abstractions;

namespace NrAcademyBL.Exceptions.EnrollmentExceptions;

public class EnrollmentAlreadyExistsException : BaseException
{
    public EnrollmentAlreadyExistsException(string message = "Tələbə artıq bu kursa qeydiyyatdan keçib!") : base(400, message)
    {
    }
}
