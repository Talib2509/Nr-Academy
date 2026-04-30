using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace NrAcademyBL.Extensions
{
    public static class FileExtensions
    {

        public static bool IsValidType(this IFormFile file, string type)
            => file.ContentType.StartsWith(type);

        public static bool IsValidSize(this IFormFile file, int kb)
            => file.Length <= kb * 1024;


        public static async Task<string> UploadAsync(this IFormFile file, params string[] paths)
        {

            string uploadPath = Path.Combine(paths);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }


            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadPath, newFileName);

            using (Stream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }


        public static void DeleteFile(string fileName, params string[] paths)
        {
            string path = Path.Combine(paths);
            string fullPath = Path.Combine(path, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}