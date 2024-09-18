using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using RealEstate.Core.Entities;

namespace RealEstate.Infraestructure.Filter
{
    /// <summary>
    /// Specifies that the class or method to which this attribute is applied requires authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called to perform authorization logic when the action is being executed.
        /// </summary>
        /// <param name="context">The <see cref="AuthorizationFilterContext"/> that provides information about the current request and response context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Throws ArgumentNullException if context is null.
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            // Check if the user is authenticated.
            if (context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, set the response as unauthorized.
                var unauthorizedCode = StatusCodes.Status401Unauthorized;

                // Set the result to a custom response result indicating that authorization failed.
                context.Result = new CustomResponseResult(
                    unauthorizedCode,
                    ReasonPhrases.GetReasonPhrase(unauthorizedCode),
                    "Authorization failed"
                );

                // Set the HTTP response status code to 401 Unauthorized.
                context.HttpContext.Response.StatusCode = unauthorizedCode;
            }
        }
    }
}
