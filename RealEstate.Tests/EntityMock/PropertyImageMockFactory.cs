using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;

namespace RealEstate.Tests.EntityMock
{
    public static class PropertyImageMockFactory
    {
        public static List<PropertyImage> CreatePropertyImages(long propertyId, int imageCount = 10)
        {
            if (propertyId <= 0)
                throw new ArgumentException("Property ID must be greater than zero.", nameof(propertyId));

            var propertyImages = new List<PropertyImage>();

            for (int i = 0; i < imageCount; i++)
            {
                propertyImages.Add(new PropertyImage
                {
                    Id = i + 1, // Assuming Ids start from 1
                    IdProperty = propertyId,
                    FileUrl = $"http://example.com/image{i + 1}.jpg",
                    Enable = i % 2 == 0 // Alternate between true and false
                });
            }

            return propertyImages;
        }

        public static List<PropertyImageCreationRequestDto> CreatePropertyImageCreationRequestDto(int propertyId, int imageCount = 5)
        {
            if (imageCount <= 0)
                throw new ArgumentException("imageCount must be greater than zero.", nameof(imageCount));

            var propertyImages = new List<PropertyImageCreationRequestDto>();

            for (int i = 0; i < imageCount; i++)
            {
                propertyImages.Add(new PropertyImageCreationRequestDto
                {
                    IdProperty = propertyId,
                    FileBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/wcAAwAB/GoT2AAAAAElFTkSuQmCC", // 1x1 black pixel in Base64
                    Enable = true, // Example value, can be customized
                    FileUrl = $"https://example.com/images/property-{propertyId}-{i}.png" // Example URL, can be ignored in most cases
                });
            }

            return propertyImages;
        }
    }
}
