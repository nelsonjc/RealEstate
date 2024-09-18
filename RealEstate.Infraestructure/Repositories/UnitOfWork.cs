using Microsoft.EntityFrameworkCore.Storage;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Infraestructure.Data;

namespace RealEstate.Infraestructure.Repositories
{
    /// <summary>
    /// Represents a unit of work that manages transactions and provides access to repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">The database context used for data access.</param>
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets the repository for <see cref="Log"/> entities.
        /// </summary>
        public IRepository<Log, long> LogRepository => new Repository<Log, long>(_dbContext);

        ///<inheritdoc/>
        public IRepository<Property, long> PropertyRepository => new Repository<Property, long>(_dbContext);
        
        ///<inheritdoc/>
        public IRepository<User, int> UserRepository => new Repository<User, int>(_dbContext);

        ///<inheritdoc/>
        public IRepository<PropertyImage, long> PropertyImageRepository => new Repository<PropertyImage, long>(_dbContext);

        /// <summary>
        /// Disposes of the resources used by the unit of work.
        /// </summary>
        public async void Dispose()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }

            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Saves all changes made in the context asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction = null;
            }
        }
    }

}
