using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyBL.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserService(UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null) throw new System.Exception("User not found.");

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }

        public async Task<string> UploadProfileImageAsync(int id, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new System.Exception("User not found.");

            if (file == null || file.Length == 0) throw new System.Exception("File is not selected or empty.");

            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
            if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            string fileUrl = $"/uploads/profiles/{uniqueFileName}";
            user.ProfileImageUrl = fileUrl;
            await _userManager.UpdateAsync(user);

            return fileUrl;
        }
    }
}
