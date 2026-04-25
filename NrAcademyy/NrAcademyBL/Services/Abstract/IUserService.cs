using Microsoft.AspNetCore.Http;
using NrAcademyBL.DTOs.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Abstract
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<string> UploadProfileImageAsync(int id, IFormFile file);
    }
}
