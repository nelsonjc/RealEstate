using RealEstate.Core.Entities;

namespace RealEstate.Core.Interfaces.Repository
{
    /// <summary>
    /// Defines the Unit of Work pattern interface, coordinating the repositories and 
    /// ensuring atomic operations across them.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for managing log entities.
        /// </summary>
        IRepository<Log, long> LogRepository { get; }

        /// <summary>
        /// Gets the repository for managing user entities.
        /// </summary>
        IRepository<User, int> UserRepository { get; }

        /// <summary>
        /// Gets the repository for managing property entities.
        /// </summary>
        IRepository<Property, long> PropertyRepository { get; }

        /// <summary>
        /// Gets the repository for managing image property entities.
        /// </summary>
        IRepository<PropertyImage, long> PropertyImageRepository { get; }


        /// <summary>
        /// Commits all changes made in the unit of work to the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation.</returns>
        Task SaveChangeAsync();
    }
}
