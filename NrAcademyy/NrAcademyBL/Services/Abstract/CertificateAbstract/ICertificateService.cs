using NrAcademyBL.DTOs.CertificateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract.CertificateAbstract;

public interface ICertificateService
{
    Task<IEnumerable<CertificateGetDTO>> GetAllAsync();
    Task<CertificateGetDTO> GetByIdAsync(int id);
    Task CreateAsync(CertificateCreateDTO dto);
    Task UpdateAsync(CertificateUpdateDTO dto);
    Task DeleteAsync(int id);
}
