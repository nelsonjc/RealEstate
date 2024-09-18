namespace RealEstate.Core.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new property.
    /// </summary>
    public class PropertyCreationRequestDto : PropertyBase
    {

        /// <summary>
        /// Gets or sets the collections of traces information associated with the property.
        /// </summary>
        public IEnumerable<PropertyTraceCreationRequestDto> Traces { get; set; }

        /// <summary>
        /// Gets or sets the collection of images associated with the property.
        /// </summary>
        public IEnumerable<PropertyImageCreationRequestDto> Images { get; set; }
    }

    /// <summary>
    /// Represents a Data Transfer Object (DTO) for updating an existing property.
    /// Inherits from <see cref="PropertyCreationRequestDto"/>.
    /// </summary>
    public class PropertyUpdateRequestDto : PropertyBase
    {
        /// <summary>
        /// Unique identifier of the property to be updated.
        /// </summary>
        public long IdProperty { get; set; }
    }

    /// <summary>
    /// Represents the base class for property-related data.
    /// This class can be extended to provide common fields for property operations.
    /// </summary>
    public class PropertyBase
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the property.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the price of the property.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the internal code for the property.
        /// </summary>
        public string CodeInternal { get; set; }

        /// <summary>
        /// Gets or sets the year of the property.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the owner of the property.
        /// </summary>
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property is active.
        /// </summary>
        public bool Active { get; set; }
    }
}
