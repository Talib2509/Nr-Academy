
using Microsoft.AspNetCore.Http;
using NrAcademyBL.DTOs.AuthDTO;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<string> UploadProfileImageAsync(int id, IFormFile file, string rootPath);
    }
}