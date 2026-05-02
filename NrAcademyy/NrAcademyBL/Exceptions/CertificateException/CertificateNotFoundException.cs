using NrAcademyBL.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Exceptions.CertificateException;

public class CertificateNotFoundException:BaseException
{
    public CertificateNotFoundException(string message = "Sertifikat tapılmadı!") : base(404, message)
    {
    }

    public CertificateNotFoundException(int id) : base(404, $"ID-si {id} olan sertifikat tapılmadı.")
    {
    }
}
