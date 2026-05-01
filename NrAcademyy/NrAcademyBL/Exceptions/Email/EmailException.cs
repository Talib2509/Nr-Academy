
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Email
{
    public class EmailException : NrAcademyException
    {
        public EmailException(string message, string errorCode = "EMAIL_ERROR")
            : base(message, StatusCodes.Status502BadGateway, errorCode)
        {
        }

        // Ən çox istifadə olunacaq factory metodlar
        public static EmailException SendingFailed(string reason)
            => new EmailException($"Email göndərilə bilmədi: {reason}", "EMAIL_SENDING_FAILED");

        public static EmailException VerificationFailed()
            => new EmailException("Email doğrulama kodu göndərilərkən xəta baş verdi.", "VERIFICATION_EMAIL_FAILED");

        public static EmailException InvalidEmailAddress(string email)
            => new EmailException($"'{email}' düzgün e-poçt ünvanı deyil.", "INVALID_EMAIL_ADDRESS");
    }
}