using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Abstractions;

public class BaseException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;
}
