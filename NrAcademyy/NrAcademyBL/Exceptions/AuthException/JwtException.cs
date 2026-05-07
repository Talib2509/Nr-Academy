// NrAcademyBL/Exceptions/Auth/JwtException.cs
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.Auth
{
    public class JwtException : BaseException
    {
        public JwtException(string message, string errorCode = "JWT_ERROR", int statusCode = StatusCodes.Status401Unauthorized)
            : base(message, statusCode, errorCode)
        {
        }

        public static JwtException InvalidRefreshToken()
            => new JwtException("Refresh token etibarsızdır və ya ləğv olunub.", "INVALID_REFRESH_TOKEN");

        public static JwtException RefreshTokenExpired()
            => new JwtException("Refresh tokenin vaxtı bitib.", "REFRESH_TOKEN_EXPIRED");

        public static JwtException TokenGenerationFailed(string reason)
            => new JwtException($"Token yaradılarkən xəta baş verdi: {reason}", "TOKEN_GENERATION_FAILED");

        public static JwtException UserNotFoundForToken(string identifier)
            => new JwtException($"'{identifier}' istifadəçisi tapılmadı.", "USER_NOT_FOUND_FOR_TOKEN", StatusCodes.Status404NotFound);
    }
}