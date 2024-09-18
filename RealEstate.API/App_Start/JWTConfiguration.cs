using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RealEstate.API.App_Start
{
    /// <summary>
    /// Class to config JWT
    /// </summary>
    internal static class JwtConfig
    {
        /// <summary>
        /// Add JWT documentation
        /// </summary>
        /// <param name="services">Parameter service type of IServiceCollection</param>
        /// <returns></returns>
        internal static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = GetTokenValidationParameters(configuration);
            });
            return services;
        }

        /// <summary>
        /// Get token validation parameters
        /// </summary>
        /// <param name="secretkey">string</param>
        /// <returns></returns>
        internal static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Auth");
            var secret = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key,
            };
        }
    }
}
