using Microsoft.Extensions.Configuration;
using RealEstate.Core.DTOs;
using RealEstate.Core.Interfaces.Utils;
using System.Drawing;
using System.Drawing.Imaging;

namespace RealEstate.Core.Services.Utils
{
    public class FileService : IFileService
    {
        private readonly string _localBasePath;
        private readonly string _libraryBasePath;
        public FileService(IConfiguration configuration)
        {
            // Ruta base para localhost (para configuración y pruebas)
            _localBasePath = configuration["FileStorage:BasePath"];

            // Ruta base dentro de la librería RealEstate.Core
            _libraryBasePath = Directory.GetCurrentDirectory();
        }

        public async Task PropertySaveMultipleImagesAsync(PropertyCreationRequestDto propertyDto)
        {
            // Crea la ruta para la nueva carpeta
            var propertyFolderName = propertyDto.CodeInternal;
            var folderPath = Path.Combine(_libraryBasePath, "Files", "PropertyImages", propertyFolderName);

            // Crea la carpeta si no existe
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Procesa las imágenes
            foreach (var image in propertyDto.Images)
            {
                if (IsValidImageBase64(image.FileBase64, out string fileExtension))
                {
                    image.FileUrl = await SaveLocalImage(image.FileBase64, propertyFolderName, folderPath, fileExtension);
                }
                else
                {
                    throw new InvalidDataException("El archivo base64 no representa una imagen válida.");
                }
            }
        }

        public async Task<string> PropertySaveAImagesAsync(PropertyImageCreationRequestDto image, string codeInternal)
        {
            string infoResult = string.Empty;
            // Crea la ruta para la nueva carpeta
            var folderPath = Path.Combine(_libraryBasePath, "Files", "PropertyImages", codeInternal);

            // Crea la carpeta si no existe
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (IsValidImageBase64(image.FileBase64, out string fileExtension))
            {
                infoResult = await SaveLocalImage(image.FileBase64, codeInternal, folderPath, fileExtension);
            }
            else
            {
                throw new InvalidDataException("El archivo base64 no representa una imagen válida.");
            }

            return infoResult;
        }

        private static async Task<string> SaveLocalImage(string fileBase64, string codeInternal, string folderPath, string fileExtension)
        {
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";
            var filePath = Path.Combine(folderPath, fileName);

            // Convierte el string base64 a un array de bytes
            var imageBytes = Convert.FromBase64String(fileBase64);

            // Guarda la imagen en el archivo
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return Path.Combine("Files", "PropertyImages", codeInternal, fileName);
        }

        public string GetFullPath(string relativePath)
        {
            // Obtiene la ruta base desde la configuración de la aplicación
            var basePath = Directory.GetCurrentDirectory();
            return Path.Combine(_localBasePath, relativePath).Replace("\\", "/");
        }

        private bool IsValidImageBase64(string base64String, out string fileExtension)
        {
            fileExtension = "jpg"; // Valor predeterminado

            try
            {
                var imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                using var image = Image.FromStream(ms);
                fileExtension = image.RawFormat.Equals(ImageFormat.Jpeg) ? "jpg" :
                                image.RawFormat.Equals(ImageFormat.Png) ? "png" :
                                image.RawFormat.Equals(ImageFormat.Gif) ? "gif" :
                                image.RawFormat.Equals(ImageFormat.Bmp) ? "bmp" :
                                null;

                if (fileExtension == null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
