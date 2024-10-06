using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CompaneyMvcPL.Helper
{
    public static class DocumentSetting
    {
        public static string  Upload (IFormFile file, string FolderName)
        {
            //Get located folder path 
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);
            //Get file name and make it unique

            string fileName= fileName = $"{Guid.NewGuid()}{file.FileName}";
            
            string FilePath = Path.Combine(FolderPath, fileName);
            //save file and stream 
            using var Fs = new FileStream(FilePath, FileMode.Create);
            // save file at stream 
            file.CopyTo(Fs);
            //return file name 
            return fileName;
        }
        public static void DeleteFile(string FileName ,string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FolderName,FileName);
            if (File.Exists(FilePath)) 
            {
                File.Delete(FilePath);
            }
        }
    }
}
