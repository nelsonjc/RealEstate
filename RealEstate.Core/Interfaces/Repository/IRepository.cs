using RealEstate.Core.Entities;
using System.Linq.Expressions;

namespace RealEstate.Core.Interfaces.Repository
{
    /// <summary>
    /// Generic repository interface for handling CRUD operations and querying entities.
    /// </summary>
    /// <typeparam name="T">The entity type to be handled by the repository.</typeparam>
    /// <typeparam name="Tkey">The type of the primary key for the entity.</typeparam>
    public interface IRepository<T, Tkey>
    {
        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The primary key of the newly added entity.</returns>
        Task<Tkey> AddAsync(T entity);

        /// <summary>
        /// Adds a collection of entities asynchronously.
        /// </summary>
        /// <param name="entities">The collection of entities to add.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        Task<bool> AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity by its primary key asynchronously.
        /// </summary>
        /// <param name="id">The primary key of the entity to retrieve.</param>
        /// <returns>The entity if found, otherwise null.</returns>
        Task<T?> GetByIdAsync(Tkey id);

        /// <summary>
        /// Retrieves entities based on a specified condition asynchronously.
        /// </summary>
        /// <param name="where">The expression to filter the entities.</param>
        /// <returns>A collection of entities that match the specified condition.</returns>
        Task<IEnumerable<T>> GetByWhereAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Retrieves entities based on a specified condition and includes related properties asynchronously.
        /// </summary>
        /// <param name="where">The expression to filter the entities.</param>
        /// <param name="properties">The related properties to include.</param>
        /// <returns>A collection of entities that match the specified condition with related properties.</returns>
        Task<IEnumerable<T>> GetByWhereWithIncludePropertyAsync(
            Expression<Func<T, bool>> where,
            Expression<Func<T, object>>[] properties);

        /// <summary>
        /// Retrieves entities based on a specified condition, includes related properties, and orders the result asynchronously.
        /// </summary>
        /// <param name="where">The expression to filter the entities.</param>
        /// <param name="properties">The related properties to include.</param>
        /// <param name="orderBy">The expression to order the entities.</param>
        /// <param name="orderAsc">Specifies whether to order the result in ascending or descending order.</param>
        /// <returns>A collection of entities that match the specified condition, with related properties and ordered by the specified expression.</returns>
        Task<IEnumerable<T>> GetByWhereWithIncludePropertyOrderByAsync(
            Expression<Func<T, bool>> where,
            Expression<Func<T, object>>[] properties,
            Expression<Func<T, Tkey>> orderBy,
            bool? orderAsc);

        /// <summary>
        /// Retrieves a paged collection of entities based on a specified condition, includes related properties, and orders the result asynchronously.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <param name="where">The expression to filter the entities.</param>
        /// <param name="properties">The related properties to include.</param>
        /// <param name="orderBy">The expression to order the entities.</param>
        /// <param name="orderAsc">Specifies whether to order the result in ascending or descending order.</param>
        /// <returns>A paged list of entities that match the specified condition, with related properties and ordered by the specified expression.</returns>
        Task<PagedList<T>> GetPagedByAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>>? where,
            Expression<Func<T, object>>[]? properties,
            Expression<Func<T, Tkey>>? orderBy,
            bool? orderAsc);

        /// <summary>
        /// Retrieves a paginated list of entities of type <typeparamref name="T"/>, including specified related properties, 
        /// with an optional ordering of the results.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="where">The filter condition to apply to the entities.</param>
        /// <param name="orderBy">An expression specifying the property to order the results by.</param>
        /// <param name="orderByAsc">Indicates whether the ordering should be ascending (true) or descending (false).</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of records to retrieve per page.</param>
        /// <param name="propertyNames">An array of expressions specifying the related properties to include in the query.</param>
        /// <returns>A <see cref="Task"/> representing an asynchronous operation that contains a paginated list of entities of type <typeparamref name="T"/>.</returns>
        Task<PagedList<T>> GetPagedListWithIncludePropertiesOrderBy(
            Expression<Func<T, bool>> where,
            Expression<Func<T, object>> orderBy,
            bool orderByAsc,
            int pageNumber,
            int pageSize,
            params Expression<Func<T, object>>[] propertyNames);


        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>A boolean indicating whether the removal was successful.</returns>
        Task<bool> RemoveAsync(T entity);

        /// <summary>
        /// Removes a collection of entities asynchronously.
        /// </summary>
        /// <param name="entities">The collection of entities to remove.</param>
        /// <returns>A boolean indicating whether the removal was successful.</returns>
        Task<bool> RemoveRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Returns an IQueryable for the entity type.
        /// </summary>
        /// <returns>An IQueryable for the entity type.</returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Saves changes to the data store asynchronously.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SaveChangeAsync();
    }
}
