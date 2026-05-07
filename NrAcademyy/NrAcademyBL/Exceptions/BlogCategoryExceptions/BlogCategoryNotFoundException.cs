using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.BlogCategoryExceptions;

public class BlogCategoryNotFoundException : BaseException
{
   
    public BlogCategoryNotFoundException(string message = "Kateqoriya tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "BLOG_CATEGORY_NOT_FOUND")
    {
    }

    public BlogCategoryNotFoundException(int id)
        : base($"ID-si {id} olan kateqoriya tapılmadı.", StatusCodes.Status404NotFound, "BLOG_CATEGORY_NOT_FOUND")
    {
    }
}