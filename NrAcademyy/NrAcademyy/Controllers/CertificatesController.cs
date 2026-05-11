using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CertificatesController : ControllerBase
    {
        private readonly ICertificateService _service;

        public CertificatesController(ICertificateService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator, Teacher")]
        public async Task<IActionResult> Create(CertificateCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok(new { Message = "Sertifikat uğurla yaradıldı." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { Message = "Sertifikat silindi." });
        }
    }
}