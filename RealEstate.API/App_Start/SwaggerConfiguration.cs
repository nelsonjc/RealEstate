using Microsoft.OpenApi.Models;
using RealEstate.Core.Constants;
using System.Reflection;

namespace RealEstate.API.App_Start
{
    /// <summary>
    /// Class to config swagger 
    /// </summary>
    internal static class SwaggerConfig
    {
        /// <summary>
        /// Add swagger configuration
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns></returns>
        internal static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ConfigConstant.APP_VERSION, GetOpenApiInfo());
                options.AddSecurityDefinition(ConfigConstant.JWT_BEARER, GetOpenApiSecurityScheme());
                options.AddSecurityRequirement(GetOpenApiSecurityRequirement());
            });
        }

        /// <summary>
        /// Get Open API information
        /// </summary>
        /// <returns></returns>
        internal static OpenApiInfo GetOpenApiInfo()
        {
            return new OpenApiInfo
            {
                Title = ConfigConstant.APP_NAME,
                Version = ConfigConstant.APP_VERSION,
                Description = ConfigConstant.JWT_DESCRIPTION,
                Contact = new OpenApiContact() { Name = ConfigConstant.APP_NAME, Email = null, Url = null }
            };
        }

        /// <summary>
        /// Get Open API security scheme
        /// </summary>
        /// <returns></returns>
        internal static OpenApiSecurityScheme GetOpenApiSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Description = ConfigConstant.SW_SEC_DEF_DESCRIPTION,
                Name = ConfigConstant.SW_SEC_NAME,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = ConfigConstant.JWT_BEARER,
                BearerFormat = "JWT"
            };
        }

        /// <summary>
        /// Get Open API security requirements
        /// </summary>
        /// <returns></returns>
        internal static OpenApiSecurityRequirement GetOpenApiSecurityRequirement()
        {
            return new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ConfigConstant.JWT_BEARER
                        }
                    }, new List<string>()
                }
            };
        }

        /// <summary>
        /// Get comments path file
        /// </summary>
        /// <returns></returns>
        internal static string GetCommentsPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string commentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}{ConfigConstant.SW_COMMENT_PATH_EXT}";
            return Path.Combine(baseDirectory, commentsFileName);
        }

        /// <summary>
        /// Use swagger configuration
        /// </summary>
        /// <param name="app"></param>
        internal static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(ConfigConstant.SW_URL_JSON, ConfigConstant.APP_NAME);
            });
        }
    }
}
