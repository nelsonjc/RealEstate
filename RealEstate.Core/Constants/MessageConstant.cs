namespace RealEstate.Core.Constants
{
    public static class MessageConstant
    {
        public const string DEFAULT_ERROR_MESSAGE = "Ha ocurrido un error, por favor intentelo de nuevo o contacte con el administrador";
        public const string LOG_ERROR_MESSAGE = "Ha ocurrido la siguiente exception: {message}";
        public const string LOG_ERROR_DETAIL_MESSAGE = "Ha ocurrido la siguiente excepción: {message}, con el  detalle: {detail}, en la hora: {time}";
        public const string PAGED_PROPERTY = "La expreasión 'property' deria ser una propiedad de la entidad";
        public const string AUTH_OK = "¡Usuario autenticado en el sistema!";
        public const string AUTH_ERROR = "Usuario o contraseña no valido.";
        public const string HASH_ERROR = "Formato hash inesperado.";
        public const string AUTHENTICATION_ERROR = "Error de autenticación.";
        public const string PROPERTY_PAGINATED_ERROR_MESSAGE = "Error en el filtro y paginación de propiedades.";
        public const string PROPERTY_GETBYID_ERROR_MESSAGE = "Error la consulta de propiedades por id.";
        public const string PROPERTY_CREATE_ERROR_MESSAGE = "Error en la creación de la propiedad.";
        public const string PROPERTY_CREATE_OK_MESSAGE = "Propiedad creada correctamente.";
        public const string PROPERTY_IMAGE_CREATE_OK_MESSAGE = "Imagen de Propiedad creada correctamente.";
        public const string PROPERTY_NOT_EXISTS_ERROR_MESSAGE = "La propiedad con el id ID_PROPERTY no existe.";
    }
}
