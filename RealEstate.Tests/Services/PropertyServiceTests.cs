using Microsoft.Extensions.Options;
using Moq;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Options;
using RealEstate.Core.QueryFilters;
using RealEstate.Core.Services.Imp;
using RealEstate.Tests.EntityMock;
using System.Linq.Expressions;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ITreeModifier> _mockTreeModifier;
        private PropertyService _propertyService;
        private PaginationOptions _paginationOptions;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTreeModifier = new Mock<ITreeModifier>();
            _paginationOptions = new PaginationOptions
            {
                DefaultPageNumber = 1,
                DefaultPageSize = 10
            };
            var options = Options.Create(_paginationOptions);
            _propertyService = new PropertyService(options, _mockTreeModifier.Object, _mockUnitOfWork.Object);
        }

        #region Add PropertyImage Tests

        [Test]
        public async Task AddImageProperty_ShouldCallAddAsyncAndSaveChangeAsync()
        {
            // Arrange
            var propertyImage = PropertyImageMockFactory.CreatePropertyImages(123, 1).First();

            _mockUnitOfWork.Setup(x => x.PropertyImageRepository.AddAsync(propertyImage))
                .ReturnsAsync(1L);

            _mockUnitOfWork.Setup(x => x.SaveChangeAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _propertyService.AddImageProperty(propertyImage);

            // Assert
            _mockUnitOfWork.Verify(x => x.PropertyImageRepository.AddAsync(propertyImage), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangeAsync(), Times.Once);
            Assert.AreEqual(1L, result);
        }

        #endregion

        #region Add Property Tests

        [Test]
        public async Task AddProperty_ShouldCallAddAsyncAndSaveChangeAsync()
        {
            // Arrange
            var property = PropertyMockFactory.CreateProperties(1).First();
            property.Images = PropertyImageMockFactory.CreatePropertyImages(property.Id, 1);
            property.Traces = PropertyTraceMockFactory.CreatePropertyTraces(property.Id, 1);

            _mockUnitOfWork.Setup(x => x.PropertyRepository.AddAsync(property))
                .ReturnsAsync(1L);

            _mockUnitOfWork.Setup(x => x.SaveChangeAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _propertyService.AddProperty(property);

            // Assert
            _mockUnitOfWork.Verify(x => x.PropertyRepository.AddAsync(property), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangeAsync(), Times.Once);
            Assert.AreEqual(1L, result);
        }

        #endregion

        #region Get By Id Tests

        [Test]
        public async Task GetById_ShouldReturnProperty_WhenFound()
        {
            // Arrange
            var property = PropertyMockFactory.CreateProperties(1).First();
            property.Images = PropertyImageMockFactory.CreatePropertyImages(property.Id, 1);
            property.Traces = PropertyTraceMockFactory.CreatePropertyTraces(property.Id, 1);

            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetByWhereWithIncludePropertyAsync(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(new List<Property> { property });

            // Act
            var result = await _propertyService.GetById(property.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(property.Id, result?.Id);
        }

        [Test]
        public async Task GetById_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetByWhereWithIncludePropertyAsync(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(new List<Property>());

            // Act
            var result = await _propertyService.GetById(1);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region Paginated Tests

        [Test]
        public async Task GetPaginated_ShouldReturnPagedList_WhenDataExists()
        {
            // Arrange
            var filter = new PropertyQueryFilter
            {
                PageNumber = 1,
                RowsOfPage = 10,
                OrderAsc = true
            };

            var properties = PropertyMockFactory.CreateProperties(2, 1, 1);
            var pagedList = new PagedList<Property>(properties, 1, 1, properties.Count);

            _mockTreeModifier.Setup(x => x.CombineAnd(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, bool>>>())).Returns((Expression<Func<Property, bool>> a, Expression<Func<Property, bool>> b) => a);
            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetPagedListWithIncludePropertiesOrderBy(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(pagedList);

            // Act
            var result = await _propertyService.GetPaginated(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(properties.Count, result.Count);
        }

        [Test]
        public async Task GetPaginated_ShouldApplyDefaultPagination_WhenFiltersAreNull()
        {
            // Arrange
            var filter = new PropertyQueryFilter(); // No filters provided
            var properties = PropertyMockFactory.CreateProperties(1);
            var pagedList = new PagedList<Property>(properties, 1, 1, properties.Count);

            _mockTreeModifier.Setup(x => x.CombineAnd(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, bool>>>())).Returns((Expression<Func<Property, bool>> a, Expression<Func<Property, bool>> b) => a);
            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetPagedListWithIncludePropertiesOrderBy(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(pagedList);

            // Act
            var result = await _propertyService.GetPaginated(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.TotalCount);
        }

        [Test]
        public async Task GetPaginated_ShouldHandleAllRowsFilter_WhenTrue()
        {
            // Arrange
            var filter = new PropertyQueryFilter
            {
                AllRows = true
            };
            var properties = PropertyMockFactory.CreateProperties(1);
            var pagedList = new PagedList<Property>(properties, 1, 1, int.MaxValue);

            _mockTreeModifier.Setup(x => x.CombineAnd(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, bool>>>())).Returns((Expression<Func<Property, bool>> a, Expression<Func<Property, bool>> b) => a);
            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetPagedListWithIncludePropertiesOrderBy(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(pagedList);

            // Act
            var result = await _propertyService.GetPaginated(filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(int.MaxValue, result.PageSize);
        }

        [Test]
        public async Task GetPaginated_ShouldHandleDefaultOrderAsc_WhenNull()
        {
            // Arrange
            var filter = new PropertyQueryFilter
            {
                OrderAsc = null
            };
            var properties = PropertyMockFactory.CreateProperties(1);
            var pagedList = new PagedList<Property>(properties, 1, 1, properties.Count);

            _mockTreeModifier.Setup(x => x.CombineAnd(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, bool>>>())).Returns((Expression<Func<Property, bool>> a, Expression<Func<Property, bool>> b) => a);
            _mockUnitOfWork.Setup(x => x.PropertyRepository.GetPagedListWithIncludePropertiesOrderBy(
                It.IsAny<Expression<Func<Property, bool>>>(),
                It.IsAny<Expression<Func<Property, object>>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Expression<Func<Property, object>>[]>()
            )).ReturnsAsync(pagedList);

            // Act
            var result = await _propertyService.GetPaginated(filter);

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}
