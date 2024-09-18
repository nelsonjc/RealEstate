using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using RealEstate.API.App_Start;
using RealEstate.API.Controllers;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.QueryFilters;
using RealEstate.Tests.EntityMock;
using System.Net;

namespace RealEstate.Tests.API
{
    [TestFixture]
    public class PropertyControllerTests
    {
        private IMapper _mapper;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IPropertyService> _propertyServiceMock;
        private PropertyController _propertyController;

        [SetUp]
        public void Setup()
        {
            _propertyServiceMock = new Mock<IPropertyService>();
            _fileServiceMock = new Mock<IFileService>();

            // Configurar AutoMapper con MappingProfile para las pruebas
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperConfig.MappingProfile(_fileServiceMock.Object));
            });

            _mapper = mapperConfig.CreateMapper();


            // Iniciar controlador con mocks
            _propertyController = new PropertyController(_mapper, _fileServiceMock.Object, _propertyServiceMock.Object);

            // Mock de HttpContext y HttpResponse para simular Response.Headers
            var mockHttpContext = new Mock<HttpContext>();
            var mockResponse = new Mock<HttpResponse>();
            var headers = new HeaderDictionary();
            mockResponse.Setup(r => r.Headers).Returns(headers);
            mockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);

            _propertyController.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
        }

        [Test]
        public async Task AddProperty_ShouldReturnCreated_WhenPropertyIsAddedSuccessfully()
        {
            // Arrange
            var requestDto = PropertyMockFactory.CreatePropertyCreationRequestDto(); // Usando Factory
            var property = PropertyMockFactory.CreateProperties(1).First(); // Usando Factory

            _fileServiceMock.Setup(f => f.PropertySaveMultipleImagesAsync(requestDto)).Returns(Task.CompletedTask);
            _propertyServiceMock.Setup(s => s.AddProperty(property)).ReturnsAsync(property.Id);

            // Act
            var result = await _propertyController.AddProperty(requestDto) as CreatedAtActionResult;

            string message = GetMessage(result);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
            Assert.AreEqual("Property successfully created", message);
        }


        [Test]
        public async Task UpdateProperty_ShouldReturnCreated_WhenPropertyIsUpdatedSuccessfully()
        {
            // Arrange
            var requestDto = PropertyMockFactory.CreatePropertyUpdateRequestDto(); // Usando Factory
            var property = PropertyMockFactory.CreateProperties(1).First(); // Usando Factory

            _propertyServiceMock.Setup(s => s.GetById(requestDto.IdProperty)).ReturnsAsync(property);
            _propertyServiceMock.Setup(s => s.UpdateProperty(property)).ReturnsAsync(property.Id);

            // Act
            var result = await _propertyController.UpdateProperty(requestDto.IdProperty, requestDto) as CreatedAtActionResult;

            string message = GetMessage(result);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
            Assert.AreEqual("Property successfully updated", message);
        }

        [Test]
        public async Task GetById_ShouldReturnOk_WhenPropertyExists()
        {
            // Arrange
            var property = PropertyMockFactory.CreateProperties(1).First(); // Usando Factory
            var propertyDto = new PropertyDto(); // Instancia de DTO

            _propertyServiceMock.Setup(s => s.GetById(property.Id)).ReturnsAsync(property);

            // Act
            var result = await _propertyController.GetById(property.Id) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task GetById_ShouldReturnNoContent_WhenPropertyDoesNotExist()
        {
            // Arrange
            var id = 1L;
            _propertyServiceMock.Setup(s => s.GetById(id)).ReturnsAsync((Property)null);

            // Act
            var result = await _propertyController.GetById(id) as NoContentResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
        }

        [Test]
        public async Task All_ShouldReturnOk_WhenPropertiesAreRetrievedSuccessfully()
        {
            // Arrange
            var queryFilter = new PropertyQueryFilter(); // Instancia de QueryFilter
            var properties = PropertyMockFactory.CreateProperties(); // Usando Factory
            var propertyDtos = new List<PropertyDto>(); // Instancia de lista de DTOs
            var paginatedResult = new PagedList<Property>(properties, 1, 10, properties.Count); // Instancia de PagedList

            _propertyServiceMock.Setup(s => s.GetPaginated(queryFilter)).ReturnsAsync(paginatedResult);

            // Act
            var result = await _propertyController.All(queryFilter) as OkObjectResult;
            var responseBody = result?.Value as dynamic;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            // Verificar que los headers fueron agregados
            Assert.True(_propertyController.Response.Headers.ContainsKey("X-Pagination"));
            var paginationHeader = _propertyController.Response.Headers["X-Pagination"];
            var metadata = JsonConvert.DeserializeObject<dynamic>(paginationHeader);
            Assert.That((int)metadata.TotalCount, Is.EqualTo(paginatedResult.TotalCount));
        }

        [Test]
        public async Task AddImageProperty_ShouldReturnOk_WhenImageIsAddedSuccessfully()
        {
            // Arrange
            var requestDto = PropertyImageMockFactory.CreatePropertyImageCreationRequestDto(1).First(); // Usando Factory
            var property = PropertyMockFactory.CreateProperties(1).First(); // Usando Factory
            var propertyImage = PropertyImageMockFactory.CreatePropertyImages(1).First(); // Usando Factory

            _propertyServiceMock.Setup(s => s.GetById(requestDto.IdProperty)).ReturnsAsync(property);
            _fileServiceMock.Setup(f => f.PropertySaveAImagesAsync(requestDto, property.CodeInternal)).ReturnsAsync(requestDto.FileUrl);
            _propertyServiceMock.Setup(s => s.AddImageProperty(propertyImage)).ReturnsAsync(1L); 

            // Act
            var result = await _propertyController.AddImageProperty(requestDto.IdProperty, requestDto) as OkObjectResult;

            string message = GetMessage(result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(MessageConstant.PROPERTY_IMAGE_CREATE_OK_MESSAGE, message);
        }

        [Test]
        public async Task AddImageProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            var requestDto = PropertyImageMockFactory.CreatePropertyImageCreationRequestDto(1).First(); // Usando Factory
            _propertyServiceMock.Setup(s => s.GetById(1)).ReturnsAsync((Property)null);

            // Act
            var result = await _propertyController.AddImageProperty(requestDto.IdProperty, requestDto) as NotFoundObjectResult;

            string message = GetMessage(result);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual(MessageConstant.PROPERTY_NOT_EXISTS_ERROR_MESSAGE.Replace("ID_PROPERTY", requestDto.IdProperty.ToString()), message);
        }

        private static string GetMessage(ObjectResult? result)
        {
            // Serializamos result.Value a JSON
            var jsonString = JsonConvert.SerializeObject(result.Value);

            // Deserializamos el JSON a un objeto dinámico
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

            // Accedemos al valor de Message
            string message = jsonObject.Message;
            return message;
        }
    }
}
