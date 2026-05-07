
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Answer
{
    public class AnswerException : BaseException
    {
        public AnswerException(string message, string errorCode = "ANSWER_ERROR", int statusCode = StatusCodes.Status400BadRequest)
            : base(message, statusCode, errorCode)
        {
        }

        public static AnswerException NotFound(int id)
            => new AnswerException($"ID: {id} olan cavab tapılmadı.", "ANSWER_NOT_FOUND", StatusCodes.Status404NotFound);
    }
}