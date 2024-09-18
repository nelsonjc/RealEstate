namespace RealEstate.Core.Entities
{

    /// <summary>
    /// A generic class that provides pagination functionality for a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the size of the page, indicating the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Indicates whether there is a previous page available.
        /// </summary>
        public bool HasPreviousPage => CurrentPage > 1;

        /// <summary>
        /// Indicates whether there is a next page available.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class with the specified items and pagination details.
        /// </summary>
        /// <param name="items">The list of items for the current page.</param>
        /// <param name="count">The total number of items across all pages.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PagedList(List<T> items, int count, int page, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
    }
}
