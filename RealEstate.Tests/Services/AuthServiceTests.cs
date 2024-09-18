using Microsoft.Extensions.Configuration;
using Moq;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Services.Imp;
using System.Linq.Expressions;

namespace RealEstate.Tests.Services
{

    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private Mock<IConfiguration> _configurationMock;
        private IAuthService _authService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordServiceMock = new Mock<IPasswordService>();
            _configurationMock = new Mock<IConfiguration>();

            _authService = new AuthService(
                _configurationMock.Object,
                _passwordServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Authenticate_ShouldReturnUser_WhenCredentialsAreValid()
        {
            // Arrange
            var authDto = new AuthDto { UserName = "testuser", Password = "password" };
            var user = new User { UserName = "testuser", Password = "hashedpassword", Active = true };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByWhereAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            _passwordServiceMock.Setup(p => p.CheckPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _authService.Authenticate(authDto);

            // Assert
            Assert.That(result.User, Is.Not.Null);
            Assert.That(result.User.UserName, Is.EqualTo(user.UserName));
            Assert.That(result.Message, Is.EqualTo(MessageConstant.AUTH_OK));
        }

        [Test]
        public async Task Authenticate_ShouldReturnError_WhenCredentialsAreInvalid()
        {
            // Arrange
            var authDto = new AuthDto { UserName = "testuser", Password = "wrongpassword" };
            var user = new User { UserName = "testuser", Password = "hashedpassword", Active = true };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByWhereAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            _passwordServiceMock.Setup(p => p.CheckPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = await _authService.Authenticate(authDto);

            // Assert
            Assert.IsNull(result.User);
            Assert.That(result.Message, Is.EqualTo(MessageConstant.AUTH_ERROR));
        }

        [Test]
        public async Task Authenticate_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var authDto = new AuthDto { UserName = "testuser", Password = "password" };

            _unitOfWorkMock.Setup(u => u.UserRepository.GetByWhereAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.Authenticate(authDto);

            // Assert
            Assert.IsNull(result.User);
            Assert.That(result.Message, Is.EqualTo(MessageConstant.AUTH_ERROR));
        }

        [Test]
        public async Task GenerateToken_ShouldReturnToken_WhenUserIsValid()
        {
            // Arrange
            var user = new User { UserName = "testuser", FullName = "Test User" };

            // La clave debe tener al menos 16 caracteres
            _configurationMock.Setup(c => c["Auth:SecretKey"]).Returns("supersecretkeythatshouldbe32byteslong!");
            _configurationMock.Setup(c => c["Auth:Issuer"]).Returns("issuer");
            _configurationMock.Setup(c => c["Auth:Audience"]).Returns("audience");
            _configurationMock.Setup(c => c["Auth:ExpirationTime"]).Returns("1");

            // Act
            var token = await _authService.GenerateToken(user);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public void GenerateToken_ShouldThrowException_WhenSecretKeyIsTooShort()
        {
            // Arrange
            var user = new User { UserName = "testuser", FullName = "Test User" };

            // Configura una clave secreta demasiado corta
            _configurationMock.Setup(c => c["Auth:SecretKey"]).Returns("shortkey123"); // Clave demasiado corta
            _configurationMock.Setup(c => c["Auth:Issuer"]).Returns("issuer");
            _configurationMock.Setup(c => c["Auth:Audience"]).Returns("audience");
            _configurationMock.Setup(c => c["Auth:ExpirationTime"]).Returns("1");

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _authService.GenerateToken(user));

            // Verifica que el mensaje de excepción contenga información sobre el tamaño insuficiente de la clave
            Assert.That(ex.Message, Does.Contain("IDX10653"));
        }
    }
}
