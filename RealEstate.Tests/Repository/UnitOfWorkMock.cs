using Moq;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;

namespace RealEstate.Tests.Repository
{
    public class UnitOfWorkMock
    {
        public static Mock<IUnitOfWork> Create()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            // Configuración de repositorios mock
            var mockLogRepository = new Mock<IRepository<Log, long>>();
            var mockPropertyRepository = new Mock<IRepository<Property, long>>();
            var mockUserRepository = new Mock<IRepository<User, int>>();
            var mockPropertyImageRepository = new Mock<IRepository<PropertyImage, long>>();

            // Configura los repositorios para que devuelvan los mocks respectivos
            mockUnitOfWork.Setup(u => u.LogRepository).Returns(mockLogRepository.Object);
            mockUnitOfWork.Setup(u => u.PropertyRepository).Returns(mockPropertyRepository.Object);
            mockUnitOfWork.Setup(u => u.UserRepository).Returns(mockUserRepository.Object);
            mockUnitOfWork.Setup(u => u.PropertyImageRepository).Returns(mockPropertyImageRepository.Object);

            // Configura los métodos de transacción
            mockUnitOfWork.Setup(u => u.SaveChangeAsync()).Returns(Task.CompletedTask);
            return mockUnitOfWork;
        }
    }
}
