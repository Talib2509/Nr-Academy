using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.Testimonial;

public class TestimonialException : BaseException
{
    public TestimonialException(
            string message,
            string errorCode = "TESTIMONIAL_ERROR",
            int statusCode = StatusCodes.Status400BadRequest)
            : base(message, statusCode, errorCode)
    {
    }

    public static TestimonialException NotFound(int id)
        => new TestimonialException(
            $"ID: {id} olan rey tapılmadı.",
            "TESTIMONIAL_NOT_FOUND",
            StatusCodes.Status404NotFound);
}

