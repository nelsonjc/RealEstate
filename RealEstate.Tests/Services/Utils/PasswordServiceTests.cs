using Microsoft.Extensions.Options;
using Moq;
using RealEstate.Core.Constants;
using RealEstate.Core.Options;
using RealEstate.Core.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Tests.Services.Utils
{
    [TestFixture]
    public class PasswordServiceTests
    {
        private PasswordService _passwordService;
        private Mock<IOptions<PasswordOptions>> _optionsMock;

        [SetUp]
        public void SetUp()
        {
            _optionsMock = new Mock<IOptions<PasswordOptions>>();

            // Configurar las opciones de la contraseña
            _optionsMock.Setup(o => o.Value).Returns(new PasswordOptions
            {
                SaltSize = 16,
                KeySize = 32,
                Iterations = 10000
            });

            // Crear instancia del servicio con las opciones mockeadas
            _passwordService = new PasswordService(_optionsMock.Object);
        }

        [Test]
        public void GenerateHash_ShouldReturnValidHash_WhenPasswordIsValid()
        {
            // Arrange
            var password = "password123";

            // Act
            var hash = _passwordService.GenerateHash(password);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(hash));
            StringAssert.Contains(".", hash);
            var parts = hash.Split('.');
            Assert.AreEqual(3, parts.Length); // Verificar que el hash tiene 3 partes (iterations, salt, key)
        }

        [Test]
        public void CheckPassword_ShouldReturnTrue_WhenPasswordMatchesHash()
        {
            // Arrange
            var password = "password123";
            var hash = _passwordService.GenerateHash(password);

            // Act
            var result = _passwordService.CheckPassword(hash, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CheckPassword_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
        {
            // Arrange
            var password = "password123";
            var hash = _passwordService.GenerateHash(password);
            var wrongPassword = "wrongpassword";

            // Act
            var result = _passwordService.CheckPassword(hash, wrongPassword);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
