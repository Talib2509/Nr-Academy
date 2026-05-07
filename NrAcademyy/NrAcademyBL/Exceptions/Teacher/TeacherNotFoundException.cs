using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Teacher;

public class TeacherNotFoundException : BaseException
{
    public TeacherNotFoundException(string message = "Müəllim tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "TEACHER_NOT_FOUND")
    {
    }

    public TeacherNotFoundException(int id)
        : base($"ID-si {id} olan müəllim tapılmadı.", StatusCodes.Status404NotFound, "TEACHER_NOT_FOUND")
    {
    }
}