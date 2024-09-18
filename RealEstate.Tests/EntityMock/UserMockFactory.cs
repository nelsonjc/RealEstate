using RealEstate.Core.Entities;

namespace RealEstate.Tests.EntityMock
{

    public static class UserMockFactory
    {
        public static User CreateUser(string password = "defaultPassword")
        {
            // Crea el mock de IPasswordService
            var passwordServiceMock = PasswordServiceMockFactory.CreatePasswordServiceMock().Object;

            // Genera un hash simulado para la contraseña
            var hashedPassword = passwordServiceMock.GenerateHash(password);

            // Crea un usuario con los datos simulados
            return new User
            {
                Id = 1,
                FullName = "John Doe",
                UserName = "johndoe",
                Password = hashedPassword,
                Active = true,
                DateCreated = DateTime.UtcNow
            };
        }
    }
}
