using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Abstractions;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.CourseException;

public class CourseNotFoundException : BaseException
{
    
    public CourseNotFoundException(string message = "Kurs tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "COURSE_NOT_FOUND")
    {
    }

    public CourseNotFoundException(int id)
        : base($"ID-si {id} olan kurs sistemdə mövcud deyil.", StatusCodes.Status404NotFound, "COURSE_NOT_FOUND")
    {
    }
}