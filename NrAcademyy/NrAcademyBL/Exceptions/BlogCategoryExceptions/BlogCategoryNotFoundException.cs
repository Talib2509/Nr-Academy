using NrAcademyBL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.BlogCategoryExceptions;

public class BlogCategoryNotFoundException:BaseException
{
    public BlogCategoryNotFoundException(string message = "Kateqoriya tapılmadı!") : base(404, message)
    {
    }

    public BlogCategoryNotFoundException(int id) : base(404, $"ID-si {id} olan kateqoriya tapılmadı.")
    {
    }
}
