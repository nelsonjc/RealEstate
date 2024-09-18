using Microsoft.Extensions.Options;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Repository;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.Options;
using RealEstate.Core.QueryFilters;
using System.Linq.Expressions;

namespace RealEstate.Core.Services.Imp
{
    public class PropertyService : IPropertyService
    {
        private readonly PaginationOptions _paginationOptions;
        private readonly ITreeModifier _treeModifier;
        private readonly IUnitOfWork _unitOfWork;

        public PropertyService(IOptions<PaginationOptions> paginationOptions, ITreeModifier treeModifier, IUnitOfWork unitOfWork)
        {
            _paginationOptions = paginationOptions.Value;
            _treeModifier = treeModifier;
            _unitOfWork = unitOfWork;
        }

        #region Public Methods
        /// <inheritdoc/>
        public async Task<long> AddImageProperty(PropertyImage entity)
        {
            await _unitOfWork.PropertyImageRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<long> AddProperty(Property entity)
        {
            await _unitOfWork.PropertyRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return entity.Id;
        }

        /// <inheritdoc/>
        public async Task<Property?> GetById(long id)
        {
            var result = await _unitOfWork.PropertyRepository.GetByWhereWithIncludePropertyAsync(
                x => x.Id == id && x.Active,
                [
                    x => x.Owner,
                    x => x.Images,
                    x => x.Traces
                ]
            );

            return result.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<PagedList<Property>> GetPaginated(PropertyQueryFilter filters)
        {
            PagedList<Property> infoResult;

            //Validation pagination
            if (filters.PageNumber == null || filters.PageNumber == 0)
                filters.PageNumber = _paginationOptions.DefaultPageNumber;

            if (filters.RowsOfPage == null || filters.RowsOfPage == 0)
                filters.RowsOfPage = _paginationOptions.DefaultPageSize;

            if (filters.AllRows)
                filters.RowsOfPage = int.MaxValue;

            if (!filters.OrderAsc.HasValue)
                filters.OrderAsc = false;

            var where = GenerateWhereFilter(filters);

            var includes = new Expression<Func<Property, object>>[]
            {
                x => x.Owner,
                x => x.Images,
                x => x.Traces
            };

            infoResult = await _unitOfWork.PropertyRepository.GetPagedListWithIncludePropertiesOrderBy(
                where,
                x => x.Id, //Order by
               filters.OrderAsc.Value,
               (int)filters.PageNumber,
               (int)filters.RowsOfPage,
               includes
            );

            return infoResult;


        }

        /// <inheritdoc/>
        public async Task<long> UpdateProperty(Property entity)
        {
            await _unitOfWork.PropertyRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangeAsync();
            return entity.Id;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Generates a dynamic 'where' filter for <see cref="Property"/> entities based on the specified <see cref="PropertyQueryFilter"/>.
        /// The filter is constructed using the provided criteria, combining conditions with logical 'AND'.
        /// </summary>
        /// <param name="filters">The <see cref="PropertyQueryFilter"/> containing the filtering criteria.</param>
        /// <returns>An <see cref="Expression{Func{T, TResult}}"/> representing the combined 'where' filter for the <see cref="Property"/> query.</returns>
        private Expression<Func<Property, bool>> GenerateWhereFilter(PropertyQueryFilter filters)
        {
            Expression<Func<Property, bool>> where = x => x.Active;

            if (!string.IsNullOrEmpty(filters.Name))
                where = _treeModifier.CombineAnd(where, x => x.Name.ToLower().Contains(filters.Name.ToLower()));

            if (!string.IsNullOrEmpty(filters.Address))
                where = _treeModifier.CombineAnd(where, x => x.Address.ToLower().Contains(filters.Address.ToLower()));

            if (filters.PriceInitial.HasValue && filters.PriceInitial.Value > 0 && filters.PriceFinish.HasValue)
                where = _treeModifier.CombineAnd(where, x => x.Price >= filters.PriceInitial.Value && x.Price <= filters.PriceFinish.Value);

            if (!string.IsNullOrEmpty(filters.CodeInternal))
                where = _treeModifier.CombineAnd(where, x => x.CodeInternal.ToLower().Contains(filters.CodeInternal.ToLower()));

            if (filters.Year.HasValue)
                where = _treeModifier.CombineAnd(where, x => x.Year == filters.Year);

            if (!string.IsNullOrEmpty(filters.OwnerName))
                where = _treeModifier.CombineAnd(where, x => x.Owner.Name.ToLower().Contains(filters.OwnerName.ToLower()));

            return where;
        }
        #endregion

    }
}
