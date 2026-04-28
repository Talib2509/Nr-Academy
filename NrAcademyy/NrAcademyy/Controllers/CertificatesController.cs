using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Services.Abstract;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(ICertificateService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(CertificateCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Sertifikat uqurla yaradıldı.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(CertificateUpdateDTO dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Sertifikat yenilendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Sertifikat silindi.");
        }
    }
}
