using RealEstate.Core.DTOs;

namespace RealEstate.Core.Interfaces.Utils
{
    public interface IFileService
    {
        Task PropertySaveMultipleImagesAsync(PropertyCreationRequestDto propertyDto);
        Task<string> PropertySaveAImagesAsync(PropertyImageCreationRequestDto image, string codeInternal);
        string GetFullPath(string relativePath);
    }
}