namespace RealEstate.Core.QueryFilters
{
    /// <summary>
    /// Represents a set of filters for querying property data, including pagination.
    /// </summary>
    public class PropertyQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Gets or sets the name of the property to filter by.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the property to filter by.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the initial price of the property for filtering by price range.
        /// </summary>
        public decimal? PriceInitial { get; set; }

        /// <summary>
        /// Gets or sets the final price of the property for filtering by price range.
        /// </summary>
        public decimal? PriceFinish { get; set; }

        /// <summary>
        /// Gets or sets the internal code used to identify the property.
        /// </summary>
        public string? CodeInternal { get; set; }

        /// <summary>
        /// Gets or sets the year of the property for filtering.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the owner's name of the property to filter by.
        /// </summary>
        public string? OwnerName { get; set; }
    }
}
