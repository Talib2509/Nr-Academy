using NrAcademyBL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.BlogPostExceptions;

public class BlogPostNotFoundException:BaseException
{
    public BlogPostNotFoundException(string message = "Blog yazısı tapılmadı!") : base(404, message)
    {
    }

    public BlogPostNotFoundException(int id) : base(404, $"ID-si {id} olan blog yazısı mövcud deyil.")
    {
    }
}
