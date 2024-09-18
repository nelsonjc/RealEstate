namespace RealEstate.Core.DTOs
{
    /// <summary>
    /// Representa un objeto de transferencia de datos (DTO) para una propiedad inmobiliaria.
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// Identificador único de la propiedad.
        /// </summary>
        public long IdProperty { get; set; }

        /// <summary>
        /// Nombre de la propiedad.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dirección física de la propiedad.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Precio de la propiedad.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Código interno de identificación de la propiedad.
        /// </summary>
        public string CodeInternal { get; set; }

        /// <summary>
        /// Año de construcción o adquisición de la propiedad.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Identificador único del propietario asociado a la propiedad.
        /// </summary>
        public long IdOwner { get; set; }

        /// <summary>
        /// Indica si la propiedad está activa o disponible.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Información del propietario de la propiedad.
        /// </summary>
        public OwnerDto Owner { get; set; }

        /// <summary>
        /// Lista de trazas históricas relacionadas con la propiedad.
        /// </summary>
        public IEnumerable<PropertyTraceDto> Traces { get; set; }

        /// <summary>
        /// Lista de imágenes asociadas con la propiedad.
        /// </summary>
        public IEnumerable<PropertyImageDto> Images { get; set; }
    }

}
