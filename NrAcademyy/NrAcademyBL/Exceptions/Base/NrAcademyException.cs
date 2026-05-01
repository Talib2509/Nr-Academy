
using Microsoft.AspNetCore.Http;

namespace NrAcademyBL.Exceptions.Base
{
    public abstract class NrAcademyException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        protected NrAcademyException(string message, int statusCode = StatusCodes.Status400BadRequest, string errorCode = "GENERAL_ERROR")
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}