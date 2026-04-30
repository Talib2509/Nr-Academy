using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; // Vacibdir

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(ICertificateService _service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous] // Sertifikatların doğruluğunu hər kəs yoxlaya bilsin
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous] // Sertifikatın detallarına baxış hər kəs üçün açıq olsun
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        // Sertifikat vermək/yaratmaq - Admin və Moderator (Nəticələrin monitorinqi çərçivəsində)
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CertificateCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Sertifikat uğurla yaradıldı.");
        }

        [HttpPut]
        // Sertifikat məlumatlarını redaktə etmək - Admin və Moderator
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(CertificateUpdateDTO dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Sertifikat yeniləndi.");
        }

        [HttpDelete("{id}")]
        // Bazadan sertifikatın silinməsi mühüm əməliyyatdır - YALNIZ Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Sertifikat silindi.");
        }
    }
}