namespace RealEstate.Core.DTOs
{
    /// <summary>
    /// Data transfer object representing the trace or record of a property's sale or transaction.
    /// </summary>
    public class PropertyTraceDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the property trace.
        /// </summary>
        public long IdPropertyTrace { get; set; }

        /// <summary>
        /// Gets or sets the date of the property sale or transaction.
        /// </summary>
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Gets or sets the name of the individual or entity involved in the transaction.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the property at the time of sale.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the tax associated with the property sale.
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated property.
        /// </summary>
        public long IdProperty { get; set; }
    }
}
