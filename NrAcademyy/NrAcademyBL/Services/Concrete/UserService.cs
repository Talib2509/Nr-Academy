using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Exceptions.User;
using NrAcademyBL.Services.Abstract;
using NrAcademyCORE.Entities.Identity;
using System;
using System.IO;
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
            if (id <= 0)
                throw new ArgumentException("İstifadəçi ID-si 0 və ya mənfi ola bilməz.", nameof(id));

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                throw UserException.NotFound(id);

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
            if (id <= 0)
                throw new ArgumentException("İstifadəçi ID-si 0 və ya mənfi ola bilməz.", nameof(id));

            // İstifadəçinin mövcudluğunu yoxlayırıq
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw UserException.NotFound(id);

            // Fayl yoxlaması
            if (file == null || file.Length == 0)
                throw UserException.ProfileImageUploadFailed("Şəkil seçilməyib və ya boşdur.");

            // Fayl ölçüsü yoxlaması (default 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB
            if (file.Length > maxFileSize)
                throw UserException.FileTooLarge(maxFileSize);

            // Fayl tipi yoxlaması
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                throw UserException.InvalidFileType();

            try
            {
                // Upload qovluğunu yaradırıq
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                // Unikal fayl adı yaradırıq
                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Faylı serverə yazırıq
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Fayl URL-ni yaradırıq
                string fileUrl = $"/uploads/profiles/{uniqueFileName}";

                // İstifadəçinin profil şəklini yeniləyirik
                user.ProfileImageUrl = fileUrl;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    // Əgər Identity update uğursuz olarsa, faylı silmək yaxşı olar (optional)
                    try { File.Delete(filePath); } catch { }
                    throw UserException.ProfileImageUploadFailed("İstifadəçi məlumatları yenilənərkən xəta baş verdi.");
                }

                return fileUrl;
            }
            catch (IOException ioEx)
            {
                throw UserException.ProfileImageUploadFailed($"Fayl sistemi xətası: {ioEx.Message}");
            }
            catch (Exception ex) when (ex is not UserException)
            {
                throw UserException.ProfileImageUploadFailed(ex.Message);
            }
        }
    }
}