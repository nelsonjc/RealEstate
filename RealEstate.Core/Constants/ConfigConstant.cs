namespace RealEstate.Core.Constants
{
    public static class ConfigConstant
    {
        public const int HOUR_DIFF = -5;
        public const string APP_NAME = "Real Estate";
        public const string APP_VERSION = "v1";
        public const string JWT_BEARER = "Bearer";
        public const string JWT_BEARER_AUTH = "bearerAuth";
        public const string JWT_FORMAT = "JWT";
        public const string JWT_DESCRIPTION = "Por favor ingrese el token aquí";

        public static string SW_SEC_NAME { get { return "Authorization"; } }
        public static string SW_URL_JSON { get { return "./v1/swagger.json"; } }
        public static string SW_COMMENT_PATH_EXT { get { return ".xml"; } }
        public static string SW_SEC_DEF_DESCRIPTION { get { return "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 1234xxxx'"; } }

    }
}
