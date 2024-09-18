using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Infraestructure.Data;

namespace RealEstate.Tests.Repository
{
    public static class RepositoryMock
    {
        public static Mock<IRepository<T, Tkey>> Create<T, Tkey>(List<T> data) where T : BaseEntity<Tkey>
        {
            var mockSet = new Mock<DbSet<T>>();
            var queryable = data.AsQueryable();

            // Set up the DbSet mock to use the queryable data.
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            // Mock methods for DbSet<T> that return ValueTask or Task
            mockSet.Setup(m => m.AddAsync(It.IsAny<T>(), default))
                .Returns((T entity, CancellationToken cancellationToken) =>
                {
                    data.Add(entity);
                    return new ValueTask<EntityEntry<T>>(new Mock<EntityEntry<T>>().Object);
                });

            mockSet.Setup(m => m.AddRangeAsync(It.IsAny<IEnumerable<T>>(), default))
                .Returns((IEnumerable<T> entities, CancellationToken cancellationToken) =>
                {
                    data.AddRange(entities);
                    return Task.CompletedTask;
                });

            mockSet.Setup(m => m.ToListAsync(default))
                .ReturnsAsync(data);

            mockSet.Setup(m => m.Where(It.IsAny<Expression<Func<T, bool>>>()))
                .Returns<Expression<Func<T, bool>>>(expression => queryable.Where(expression).AsQueryable());

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Set<T>()).Returns(mockSet.Object);

            var mockRepository = new Mock<IRepository<T, Tkey>>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<T>())).ReturnsAsync((T entity) =>
            {
                data.Add(entity);
                return entity.Id;
            });

            mockRepository.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<T>>())).ReturnsAsync(true);
            mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(data);
            mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Tkey>())).ReturnsAsync((Tkey id) => data.FirstOrDefault(e => e.Id.Equals(id)));
            mockRepository.Setup(r => r.GetByWhereAsync(It.IsAny<Expression<Func<T, bool>>>())).ReturnsAsync((Expression<Func<T, bool>> where) => data.AsQueryable().Where(where).ToList());
            mockRepository.Setup(r => r.GetPagedByAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<Expression<Func<T, object>>[]>(), It.IsAny<Expression<Func<T, Tkey>>>(), It.IsAny<bool?>())).ReturnsAsync((int page, int pageSize, Expression<Func<T, bool>> where, Expression<Func<T, object>>[] properties, Expression<Func<T, Tkey>> orderBy, bool? orderAsc) =>
            {
                var query = data.AsQueryable();
                if (where != null) query = query.Where(where);
                if (orderAsc.HasValue) query = orderAsc.Value ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
                var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return new PagedList<T>(items, query.Count(), page, pageSize);
            });

            return mockRepository;
        }
    }
}
