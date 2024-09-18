using RealEstate.Core.Entities;
using RealEstate.Core.QueryFilters;

namespace RealEstate.Core.Interfaces.Services
{
    /// <summary>
    /// Defines the contract for property management operations, including adding, retrieving, and paginating properties.
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Adds a new property entity to the database.
        /// </summary>
        /// <param name="entity">The property entity to add.</param>
        /// <returns>The ID of the newly added property.</returns>
        Task<long> AddProperty(Property entity);

        /// <summary>
        /// Adds a new property image entity to the database.
        /// </summary>
        /// <param name="entity">The property image entity to add.</param>
        /// <returns>The ID of the newly added property image.</returns>
        Task<long> AddImageProperty(PropertyImage entity);

        /// <summary>
        /// Retrieves a property entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property.</param>
        /// <returns>The property entity if found, or null if not found.</returns>
        Task<Property?> GetById(long id);

        /// <summary>
        /// Retrieves a paginated list of properties based on the provided filters.
        /// </summary>
        /// <param name="filters">The filters to apply for retrieving the paginated list of properties.</param>
        /// <returns>A paginated list of properties that match the specified filters.</returns>
        Task<PagedList<Property>> GetPaginated(PropertyQueryFilter filters);

        /// <summary>
        /// Update  a property entity to the database.
        /// </summary>
        /// <param name="entity">The property entity to add.</param>
        /// <returns>The ID of the newly added property.</returns>
        Task<long> UpdateProperty(Property entity);
    }
}
