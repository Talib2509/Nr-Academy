
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.TestResult
{
    public class TestResultException : NrAcademyException
    {
        public TestResultException(string message, string errorCode = "TESTRESULT_ERROR", int statusCode = StatusCodes.Status400BadRequest)
            : base(message, statusCode, errorCode)
        {
        }

        public static TestResultException NotFound(int id)
            => new TestResultException($"ID: {id} olan nəticə tapılmadı.", "TESTRESULT_NOT_FOUND", StatusCodes.Status404NotFound);
    }
}