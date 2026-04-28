using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities.Identity;
using NrAcademyBL.Extensions;

namespace NrAcademyBL.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new Exception("İstifadəçi tapılmadı.");

           
            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> UploadProfileImageAsync(int id, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new Exception("İstifadəçi tapılmadı.");

           
            if (!file.IsValidType("image/")) throw new Exception("Yalnız şəkil yükləyə bilərsiniz.");
            if (!file.IsValidSize(2048)) throw new Exception("Şəkil ölçüsü 2MB-dan çox olmamalıdır.");

            // 2. Köhnə şəkli serverdən silmək
            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                string oldFileName = Path.GetFileName(user.ProfileImageUrl);
                FileExtensions.DeleteFile(oldFileName, _env.WebRootPath, "uploads", "profiles");
            }

            // 3. Yeni şəkli yükləmək
            string newFileName = await file.UploadAsync(_env.WebRootPath, "uploads", "profiles");

            // 4. Verilənlər bazasını yeniləmək
            user.ProfileImageUrl = $"/uploads/profiles/{newFileName}";
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new Exception("Şəkil yenilənərkən xəta baş verdi.");

            return user.ProfileImageUrl;
        }
    }
}