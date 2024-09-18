namespace RealEstate.Core.QueryFilters
{
    /// <summary>
    /// Represents the query parameters used for pagination and sorting in requests.
    /// </summary>
    public class PaginationQueryFilter
    {
        /// <summary>
        /// Gets or sets the page number for pagination. 
        /// If null, the default value is typically the first page.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of rows per page for pagination.
        /// If null, a default number of rows may be used.
        /// </summary>
        public int? RowsOfPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sorting order should be ascending.
        /// If null, the default sorting order may be used.
        /// </summary>
        public bool? OrderAsc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all rows should be returned, ignoring pagination.
        /// If set to true, pagination is bypassed and all rows are returned.
        /// </summary>
        public bool AllRows { get; set; }
    }

}
