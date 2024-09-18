using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Constants;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Infraestructure.Data;
using System.Linq.Expressions;

namespace RealEstate.Infraestructure.Repositories
{
    /// <summary>
    /// Represents a repository that provides data access operations for entities of type <typeparamref name="T"/> 
    /// with a primary key of type <typeparamref name="Tkey"/>.
    /// </summary>
    /// <typeparam name="T">The type of entity the repository manages. Must be a subclass of <see cref="BaseEntity{Tkey}"/>.</typeparam>
    /// <typeparam name="Tkey">The type of the primary key for the entity.</typeparam>
    public class Repository<T, Tkey> : IRepository<T, Tkey> where T : BaseEntity<Tkey>
    {
        #region Variables

        /// <summary>
        /// The database context used for data access.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// The DbSet for the entity type <typeparamref name="T"/>.
        /// </summary>
        internal readonly DbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T, Tkey}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for data access.</param>
        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }

        #endregion

        /// <inheritdoc/>
        public async Task<Tkey> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
            return true;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        /// <inheritdoc/>
        public async Task<T?> GetByIdAsync(Tkey id) => await _entities.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByWhereAsync(Expression<Func<T, bool>> where)
        {
            return await _dbContext.Set<T>().Where(where).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByWhereWithIncludePropertyAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = where == null ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().Where(where).AsQueryable();

            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    if (property.Body is MemberExpression member)
                    {
                        query = query.Include(property);
                    }
                    else
                    {
                        throw new InvalidOperationException(MessageConstant.PAGED_PROPERTY);
                    }
                }
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetByWhereWithIncludePropertyOrderByAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>>[] properties, Expression<Func<T, Tkey>> orderBy, bool? orderAsc)
        {
            IQueryable<T> query = where == null ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().Where(where).AsQueryable();

            query = orderAsc.HasValue && orderAsc.Value ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    if (property.Body is MemberExpression member)
                    {
                        query = query.Include(property);
                    }
                    else
                    {
                        throw new InvalidOperationException(MessageConstant.PAGED_PROPERTY);
                    }
                }
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<PagedList<T>> GetPagedByAsync(int page, int pageSize, Expression<Func<T, bool>>? where, Expression<Func<T, object>>[]? properties, Expression<Func<T, Tkey>>? orderBy, bool? orderAsc)
        {
            IQueryable<T> query;
            int totalRecord;

            query = where == null ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().Where(where).AsQueryable();
            totalRecord = await query.CountAsync();

            query = orderAsc.HasValue && orderAsc.Value ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            if (properties != null && properties.Any())
            {
                foreach (var property in properties)
                {
                    if (property.Body is MemberExpression member)
                    {
                        query = query.Include(property);
                    }
                    else
                    {
                        throw new InvalidOperationException(MessageConstant.PAGED_PROPERTY);
                    }
                }
            }

            var items = await query.ToListAsync();
            return new PagedList<T>(items, totalRecord, page, pageSize);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public IQueryable<T> AsQueryable() => _dbContext.Set<T>().AsQueryable();

        /// <inheritdoc/>
        public async Task SaveChangeAsync() => await _dbContext.SaveChangesAsync();

        /// <inheritdoc/>
        public async Task<PagedList<T>> GetPagedListWithIncludePropertiesOrderBy(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderBy, bool orderByAsc, int pageNumber, int pageSize, params Expression<Func<T, object>>[] propertyNames)
        {
            IQueryable<T> query;
            int totalItems;

            query = where is null
                ? _dbContext.Set<T>().AsQueryable()
                : _dbContext.Set<T>().Where(where).AsQueryable();

            totalItems = await query.CountAsync();

            query = orderByAsc
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if (propertyNames != null)
            {
                query = ValidateInclude(propertyNames, query);
            }

            try
            {
                var items = await query.ToListAsync();
                return new PagedList<T>(items, totalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates and applies the specified property expressions for inclusion in the query.
        /// Supports both simple property access and nested property access.
        /// </summary>
        /// <param name="propertyNames">An array of expressions specifying the properties to include in the query.</param>
        /// <param name="query">The <see cref="IQueryable{T}"/> query to which the include expressions will be applied.</param>
        /// <returns>The modified <see cref="IQueryable{T}"/> with the specified properties included.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when one of the expressions does not represent a valid property access.
        /// </exception>
        private IQueryable<T> ValidateInclude(Expression<Func<T, object>>[] propertyNames, IQueryable<T> query)
        {
            foreach (var item in propertyNames)
            {
                if (item.Body is MemberExpression memberExpression)
                {
                    query = query.Include(item);
                }
                else if (item.Body is MethodCallExpression methodCallExpression)
                {
                    query = IncludeNestedProperty(query, methodCallExpression);
                }
                else
                {
                    throw new InvalidOperationException("The expression 'item' should represent a property access.");
                }
            }

            return query;
        }

        /// <summary>
        /// Includes a nested property in the query based on the provided method call expression.
        /// Constructs the include path by recursively traversing the expression tree.
        /// </summary>
        /// <param name="query">The <see cref="IQueryable{T}"/> query to which the nested property will be included.</param>
        /// <param name="expression">The <see cref="MethodCallExpression"/> representing the nested property.</param>
        /// <returns>The modified <see cref="IQueryable{T}"/> with the nested property included.</returns>
        private IQueryable<T> IncludeNestedProperty(IQueryable<T> query, MethodCallExpression expression)
        {
            var includePath = string.Join(".", GetIncludePath(expression));
            return query.Include(includePath);
        }


        /// <summary>
        /// Recursively retrieves the include path for a nested property by traversing the expression tree.
        /// </summary>
        /// <param name="expression">The <see cref="Expression"/> representing the property chain.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/> representing the path to the nested property.</returns>
        private IEnumerable<string> GetIncludePath(Expression expression)
        {
            while (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Arguments.FirstOrDefault() is MemberExpression memberExpression)
                {
                    yield return memberExpression.Member.Name;
                    expression = methodCallExpression.Arguments.LastOrDefault();
                }
            }

            if (expression is MemberExpression memberExpressionFinal)
            {
                yield return memberExpressionFinal.Member.Name;
            }
        }

    }
}
