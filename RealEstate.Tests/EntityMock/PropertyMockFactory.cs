using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;

namespace RealEstate.Tests.EntityMock
{

    public static class PropertyMockFactory
    {
        public static List<Property> CreateProperties(int propertyCount = 10, int imageCount = 10, int traceCount = 5)
        {
            if (propertyCount <= 0)
                throw new ArgumentException("propertyCount must be greater than zero.", nameof(propertyCount));

            var properties = new List<Property>();

            for (int i = 0; i < propertyCount; i++)
            {
                var propertyId = i + 1; // Assuming Ids start from 1

                // Create the Owner and PropertyImage mocks
                var owner = OwnerMockFactory.CreateOwners(1).First(); 
                var images = PropertyImageMockFactory.CreatePropertyImages(propertyId, imageCount);
                var traces = PropertyTraceMockFactory.CreatePropertyTraces(propertyId, traceCount);

                var property = new Property
                {
                    Id = propertyId,
                    Name = $"Property {propertyId}",
                    Address = $"Address {propertyId}",
                    Price = 100000m + (propertyId * 1000), // Example price
                    CodeInternal = $"Code{propertyId}",
                    Year = 2000 + propertyId, // Example year
                    IdOwner = owner.Id, // Assign the mock owner's ID
                    Active = true,
                    Owner = owner,
                    Images = images,
                    Traces = traces
                };

                properties.Add(property);
            }

            return properties;
        }

        public static PropertyCreationRequestDto CreatePropertyCreationRequestDto(int propertyCount = 1, int imageCount = 10,int traceCount = 5)
        {
            if (propertyCount <= 0)
                throw new ArgumentException("propertyCount must be greater than zero.", nameof(propertyCount));

            var propertyId = 1; // Fixed ID for simplicity

            var images = PropertyImageMockFactory.CreatePropertyImages(propertyId, imageCount);
            var traces = PropertyTraceMockFactory.CreatePropertyTraces(propertyId, traceCount);

            return new PropertyCreationRequestDto
            {
                Name = $"Property {propertyId}",
                Address = $"Address {propertyId}",
                Price = 100000m + (propertyId * 1000), // Example price
                CodeInternal = $"Code{propertyId}",
                Year = 2000 + propertyId, // Example year
                IdOwner = 1, // Example owner ID
                Active = true,
                Images = images.Select(img => new PropertyImageCreationRequestDto
                {
                    IdProperty = propertyId,
                    FileBase64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/wcAAwAB/GoT2AAAAAElFTkSuQmCC",
                    Enable = img.Enable
                }).ToList(),
                Traces = traces.Select(trace => new PropertyTraceCreationRequestDto
                {
                    DateSale = trace.DateSale,
                    Name = trace.Name,
                    Value = trace.Value,
                    Tax = trace.Tax,
                    IdProperty = trace.IdProperty
                }).ToList()
            };
        }

        public static PropertyUpdateRequestDto CreatePropertyUpdateRequestDto(int propertyCount = 1, int imageCount = 10, int traceCount = 5)
        {
            if (propertyCount <= 0)
                throw new ArgumentException("propertyCount must be greater than zero.", nameof(propertyCount));

            var propertyId = 1; // Fixed ID for simplicity

            return new PropertyUpdateRequestDto
            {
                IdProperty = propertyId,
                Name = $"Property {propertyId}",
                Address = $"Address {propertyId}",
                Price = 100000m + (propertyId * 1000), // Example price
                CodeInternal = $"Code{propertyId}",
                Year = 2000 + propertyId, // Example year
                IdOwner = 1, // Example owner ID
                Active = true
            };
        }
    }
}
