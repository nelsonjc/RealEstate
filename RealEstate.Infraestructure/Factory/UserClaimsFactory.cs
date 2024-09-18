using System.IdentityModel.Tokens.Jwt;

namespace RealEstate.Infraestructure.Factory
{
    /// <summary>
    /// A factory class for extracting user claims from authorization tokens.
    /// </summary>
    public class UserClaimsFactory
    {
        /// <summary>
        /// Constant representing the token type prefix "Bearer ".
        /// </summary>
        private const string VALUE_BEARER = "Bearer ";

        /// <summary>
        /// Extracts the user ID from the provided authorization header.
        /// </summary>
        /// <param name="authHeader">The authorization header containing the token. Expected to start with "Bearer ".</param>
        /// <returns>The user ID extracted from the JWT token, or null if extraction fails or the token is invalid.</returns>
        public static string GetUserIdFromToken(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith(VALUE_BEARER, StringComparison.OrdinalIgnoreCase))
            {
                return null!;
            }

            var token = authHeader[VALUE_BEARER.Length..].Trim();
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken || !jwtToken.Payload.TryGetValue("User", out var value))
            {
                return null!;
            }

            var user = value.ToString();
            return user!;
        }
    }

}
