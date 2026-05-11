using Microsoft.AspNetCore.Mvc;
using NrAcademyBL.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using NrAcademyBL.DTOs.TestResultDTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NrAcademyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultService _service;

        public TestResultsController(ITestResultService service)
        {
            _service = service;
        }

        [HttpPost("Submit")]
        public async Task<IActionResult> Submit(TestSubmitDto dto)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var result = await _service.SubmitTestAsync(int.Parse(userIdStr), dto);
            return Ok(new { Message = "İmtahan nəticəniz hesablandı", Data = result });
        }

        




        [HttpGet("ByTest/{testId}")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> GetByTest(int testId)
        {
            return Ok(await _service.GetResultsByTestIdAsync(testId));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator, Teacher")]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpPost("DetermineWinner/{testId}")]
        [Authorize(Roles = "Admin, Moderator, Teacher")]
        public async Task<IActionResult> DetermineWinner(int testId)
        {
            await _service.DetermineWinnerForTestAsync(testId);
            return Ok(new { Message = "Günün qalibi təsdiqləndi!" });
        }

        [HttpGet("MyResults")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(await _service.GetUserResultsAsync(userId));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}