namespace RealEstate.Core.DTOs
{
    /// <summary>
    /// Data transfer object representing an owner with related details.
    /// </summary>
    public class OwnerDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the owner.
        /// </summary>
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the owner.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the URL or path to the owner's photo.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets the birthdate of the owner.
        /// </summary>
        public DateTime Birthday { get; set; }
    }

}
