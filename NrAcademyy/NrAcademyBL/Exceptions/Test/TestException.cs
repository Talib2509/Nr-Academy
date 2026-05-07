
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Test
{
    public class TestException : BaseException
    {
        public TestException(string message, string errorCode = "TEST_ERROR", int statusCode = StatusCodes.Status400BadRequest)
            : base(message, statusCode, errorCode)
        {
        }

        public static TestException NotFound(int id)
            => new TestException($"ID: {id} olan test tapılmadı.", "TEST_NOT_FOUND", StatusCodes.Status404NotFound);
    }
}