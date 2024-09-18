using Moq;
using RealEstate.API.Controllers;
using RealEstate.Core.DTOs;
using RealEstate.Core.Exceptions;
using RealEstate.Core.Interfaces.Services;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.Constants;
using RealEstate.Core.Entities;

namespace RealEstate.Tests.API
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        [Test]
        public async Task Authenticate_ShouldReturnOk_WhenAuthenticationIsSuccessful()
        {
            // Arrange
            var authDto = new AuthDto { UserName = "testuser", Password = "password123" };
            var user = new User
            {
                UserName = authDto.UserName,
                Password = "hashedPassword" // Simulate a hashed password
            };
            var token = "generatedToken";

            _authServiceMock.Setup(s => s.Authenticate(authDto))
                .ReturnsAsync((user, "Success"));

            _authServiceMock.Setup(s => s.GenerateToken(user))
                .ReturnsAsync(token);

            // Act
            var result = await _authController.Authenticate(authDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Authenticate_ShouldThrowBusinessException_WhenAuthenticationFails()
        {
            // Arrange
            var authDto = new AuthDto { UserName = "testuser", Password = "wrongPassword" };

            _authServiceMock.Setup(s => s.Authenticate(authDto))
                .ReturnsAsync((null, "Invalid credentials"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<BusinessException>(async () =>
                await _authController.Authenticate(authDto));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Description, Is.EqualTo(MessageConstant.AUTHENTICATION_ERROR));
            Assert.That(exception.Message, Is.EqualTo("Invalid credentials"));
        }
    }
}
