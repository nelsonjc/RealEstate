namespace RealEstate.API.App_Start
{
    /// <summary>
    /// Configura la obtención de cadenas de conexión basadas en el entorno actual (desarrollo, producción, etc.).
    /// También agrega soporte para variables de entorno y secretos de usuario.
    /// </summary>
    public class SwitchConnectionConfiguration
    {
        // Campos estáticos para almacenar la configuración y el entorno actual
        private static readonly IConfigurationRoot _configuration;
        private static readonly string _env;

        /// <summary>
        /// Constructor estático que inicializa la configuración cargando diferentes archivos
        /// de configuración dependiendo del entorno actual de la aplicación (Development, Production, etc.).
        /// Incluye también la carga de variables de entorno y secretos de usuario.
        /// </summary>
        static SwitchConnectionConfiguration()
        {
            // Obtiene el entorno de la variable de entorno "ASPNETCORE_ENVIRONMENT", por defecto es "Development".
            _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Construye la configuración a partir de archivos JSON, secretos de usuario y variables de entorno.
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Establece la base desde la cual se busca el archivo.
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Archivo base de configuración.
                .AddJsonFile($"appsettings.{_env}.json", optional: true, reloadOnChange: true) // Configuración específica del entorno.
                .AddUserSecrets<SwitchConnectionConfiguration>() // Carga secretos de usuario.
                .AddEnvironmentVariables(); // Agrega las variables de entorno.

            _configuration = builder.Build(); // Construye el objeto de configuración.
        }

        /// <summary>
        /// Obtiene la cadena de conexión desde los archivos de configuración o las variables de entorno.
        /// Prioriza la cadena de conexión en "appsettings.json", luego revisa las variables de entorno.
        /// </summary>
        /// <returns>
        /// La cadena de conexión obtenida o null si no se encuentra.
        /// </returns>
        public static string? GetConnection()
        {
            // Intenta obtener la cadena de conexión desde los archivos de configuración.
            string? conn = _configuration.GetConnectionString("DataBaseConnection");

            // Si no se encuentra en los archivos de configuración, intenta obtenerla desde las variables de entorno.
            if (string.IsNullOrEmpty(conn))
            {
                conn = Environment.GetEnvironmentVariable("DataBaseConnection");
            }

            // Retorna la cadena de conexión obtenida o null si no se encuentra.
            return conn;
        }
    }

}
