namespace RealEstate.Core.DTOs
{
    /// <summary>
    /// Data transfer object representing an image associated with a property.
    /// </summary>
    public class PropertyImageDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the property image.
        /// </summary>
        public long IdPropertyImage { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the property associated with this image.
        /// </summary>
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the URL or path to the image file.
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image is enabled or active.
        /// </summary>
        public bool Enable { get; set; }
    }
}
