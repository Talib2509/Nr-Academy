
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NrAcademyBL.DTOs.AuthDTO;
using NrAcademyBL.Exceptions.User;
using NrAcademyBL.Extensions;
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

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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

        public async Task<string> UploadProfileImageAsync(int id, IFormFile file, string rootPath)
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

            // Fayl tipinin şəkil olub-olmadığını yoxlayırıq
            if (!file.IsValidType("image"))
                throw UserException.InvalidFileType();

            // Faylın ölçüsünün 5MB-dan böyük olub-olmadığını yoxlayırıq
            if (!file.IsValidSize(5000))
                throw UserException.FileTooLarge();

            try
            {
                // Əgər istifadəçinin köhnə profili varsa, serverdən silirik
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    FileExtensions.DeleteFile(Path.GetFileName(user.ProfileImageUrl), rootPath, "uploads", "profiles");
                }

                // FileExtensions vasitəsilə yeni faylı yükləyirik
                string uploadFolder = Path.Combine(rootPath, "uploads", "profiles");
                string newFileName = await file.UploadAsync(uploadFolder);

                // Fayl URL-ni yaradırıq
                string fileUrl = $"/uploads/profiles/{newFileName}";

                // İstifadəçinin profil şəklini yeniləyirik
                user.ProfileImageUrl = fileUrl;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    // Əgər Identity update uğursuz olarsa, yüklənmiş faylı silirik
                    FileExtensions.DeleteFile(newFileName, rootPath, "uploads", "profiles");
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