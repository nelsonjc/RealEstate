using Microsoft.Extensions.Configuration;
using Moq;
using RealEstate.Core.DTOs;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Tests.Services.Utils
{
    [TestFixture]
    public class FileServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private IFileService _fileService;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();

            // Configuración de prueba
            _configurationMock.Setup(c => c["FileStorage:BasePath"]).Returns("TestBasePath");

            // Inicialización del servicio
            _fileService = new FileService(_configurationMock.Object);
        }


        [Test]
        public void PropertySaveMultipleImagesAsync_ShouldThrowInvalidDataException_WhenInvalidBase64()
        {
            // Arrange
            var propertyDto = new PropertyCreationRequestDto
            {
                CodeInternal = "TestPropertyCode",
                Images = new List<PropertyImageCreationRequestDto>
                {
                    new PropertyImageCreationRequestDto
                    {
                        FileBase64 = "invalid_base64"
                    }
                }
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(async () =>
                await _fileService.PropertySaveMultipleImagesAsync(propertyDto));
        }


        [Test]
        public void PropertySaveAImagesAsync_ShouldThrowInvalidDataException_WhenInvalidBase64()
        {
            // Arrange
            var imageDto = new PropertyImageCreationRequestDto
            {
                FileBase64 = "invalid_base64"
            };
            var codeInternal = "TestPropertyCode";

            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(async () =>
                await _fileService.PropertySaveAImagesAsync(imageDto, codeInternal));
        }

        [Test]
        public void GetFullPath_ShouldReturnFullPath()
        {
            // Arrange
            var relativePath = "Files/PropertyImages/TestImage.jpg";
            var expectedPath = Path.Combine("TestBasePath", relativePath).Replace("\\", "/");

            // Act
            var result = _fileService.GetFullPath(relativePath);

            // Assert
            Assert.AreEqual(expectedPath, result);
        }
    }
}
