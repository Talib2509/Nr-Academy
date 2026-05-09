using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.DTOs.CertificateDTO;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization; 

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController(ICertificateService _service) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous] 
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
       
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Create(CertificateCreateDTO dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Sertifikat uğurla yaradıldı.");
        }

        [HttpPut]
      
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Update(CertificateUpdateDTO dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Sertifikat yeniləndi.");
        }

        [HttpDelete("{id}")]
      
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Sertifikat silindi.");
        }
    }
}