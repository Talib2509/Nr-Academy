using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Abstractions;

public interface IBaseException
{
    int StatusCode { get; }
    string ErrorCode { get; }
}