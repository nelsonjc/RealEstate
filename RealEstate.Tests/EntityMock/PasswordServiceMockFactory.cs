using Moq;
using RealEstate.Core.Interfaces.Utils;

namespace RealEstate.Tests.EntityMock
{
    public static class PasswordServiceMockFactory
    {
        public static Mock<IPasswordService> CreatePasswordServiceMock()
        {
            var passwordServiceMock = new Mock<IPasswordService>();

            // Configura el mock para el método GenerateHash
            passwordServiceMock
                .Setup(ps => ps.GenerateHash(It.IsAny<string>()))
                .Returns((string password) => GenerateMockHash(password));

            // Configura el mock para el método CheckPassword
            passwordServiceMock
                .Setup(ps => ps.CheckPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string hash, string password) => CheckMockPassword(hash, password));

            return passwordServiceMock;
        }

        private static string GenerateMockHash(string password)
        {
            // Simula un hash simple. Aquí puedes usar una lógica más compleja si lo deseas.
            var salt = "mockSalt";
            var key = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + salt));
            return $"{1000}.{salt}.{key}";
        }

        private static bool CheckMockPassword(string hash, string password)
        {
            var parts = hash.Split('.');

            if (parts.Length != 3)
                throw new FormatException("Hash format is invalid.");

            var expectedKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + parts[1]));
            return parts[2] == expectedKey;
        }
    }

}
