using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.BlogPostExceptions;

public class BlogPostNotFoundException : BaseException
{
    
    public BlogPostNotFoundException(string message = "Blog yazısı tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "BLOG_POST_NOT_FOUND")
    {
    }

    public BlogPostNotFoundException(int id)
        : base($"ID-si {id} olan blog yazısı mövcud deyil.", StatusCodes.Status404NotFound, "BLOG_POST_NOT_FOUND")
    {
    }
}