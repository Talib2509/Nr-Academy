using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Question
{
    public class QuestionException : BaseException
    {
        public QuestionException(string message, string errorCode = "QUESTION_ERROR", int statusCode = StatusCodes.Status400BadRequest)
            : base(message, statusCode, errorCode)
        {
        }

        public static QuestionException NotFound(int id)
            => new QuestionException($"ID: {id} olan sual tapılmadı.", "QUESTION_NOT_FOUND", StatusCodes.Status404NotFound);
    }
}