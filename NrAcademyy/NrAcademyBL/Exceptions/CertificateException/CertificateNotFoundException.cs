using Microsoft.AspNetCore.Http; 
using NrAcademyBL.Exceptions.Base;

namespace NrAcademyBL.Exceptions.CertificateException;

public class CertificateNotFoundException : BaseException
{
    // Ardıcıllıq: base(message, statusCode, errorCode)
    public CertificateNotFoundException(string message = "Sertifikat tapılmadı!")
        : base(message, StatusCodes.Status404NotFound, "CERTIFICATE_NOT_FOUND")
    {
    }

    public CertificateNotFoundException(int id)
        : base($"ID-si {id} olan sertifikat tapılmadı.", StatusCodes.Status404NotFound, "CERTIFICATE_NOT_FOUND")
    {
    }
}