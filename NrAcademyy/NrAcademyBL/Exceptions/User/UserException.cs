// NrAcademyBL/Exceptions/User/UserException.cs
using Microsoft.AspNetCore.Http;
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.User
{
    public class UserException :BaseException
    {
        public UserException(string message, string errorCode = "USER_ERROR", int statusCode = StatusCodes.Status400BadRequest)
             : base(message, statusCode, errorCode)
        {
        }

        public static UserException NotFound(int id)
            => new UserException($"ID: {id} olan istifadəçi tapılmadı.", "USER_NOT_FOUND", StatusCodes.Status404NotFound);

        public static UserException NotFound(string userNameOrEmail)
            => new UserException($"'{userNameOrEmail}' adlı istifadəçi tapılmadı.", "USER_NOT_FOUND", StatusCodes.Status404NotFound);

        public static UserException ProfileImageUploadFailed(string reason)
            => new UserException($"Profil şəkli yüklənərkən xəta baş verdi: {reason}", "PROFILE_IMAGE_UPLOAD_FAILED");

        public static UserException InvalidFileType()
            => new UserException("Yalnız JPG, JPEG və PNG formatlı şəkillər yüklənə bilər.", "INVALID_FILE_TYPE");

        public static UserException FileTooLarge(long maxSizeInBytes = 5 * 1024 * 1024)
            => new UserException($"Şəkil həcmi {maxSizeInBytes / (1024 * 1024)} MB-dan çox ola bilməz.", "FILE_TOO_LARGE");
    }
}