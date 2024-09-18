using Microsoft.AspNetCore.Mvc.Filters;
using RealEstate.Core.Constants;
using RealEstate.Core.Entities;
using System.Net;
using RealEstate.Infraestructure.Factory;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Core.Interfaces.Repository;

namespace RealEstate.Infraestructure.Filter
{
    /// <summary>
    /// Custom action filter attribute for logging request information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LogActionFilter : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// Asynchronously executes the action and logs the request information.
        /// </summary>
        /// <param name="context">The context of the current executing action.</param>
        /// <param name="next">Delegate to execute the next action in the pipeline.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execute the action
            await next();

            // Log request info asynchronously
            _ = LogRquestInfoAsync(context);
        }

        /// <summary>
        /// Asynchronously logs information about the current HTTP request.
        /// </summary>
        /// <param name="context">The context of the current executing action.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        private static async Task LogRquestInfoAsync(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var ip = context.HttpContext.Connection.RemoteIpAddress;
            var headers = request.Headers;

            string user = null!;

            // Extract user information from authorization header
            if (headers.TryGetValue("Authorization", out var header))
            {
                var authHeader = header.ToString();
                user = UserClaimsFactory.GetUserIdFromToken(authHeader);
            }

            // Create the log message
            var obj = Message(user, request.Path.ToString(), request.Method, ip!.ToString());

            // Get the Unit of Work and log the information
            var unitOfWork = context.HttpContext.RequestServices.GetService<IUnitOfWork>();
            await unitOfWork.LogRepository!.AddAsync(new Log { Message = obj, DateLog = DateTime.UtcNow.AddHours(-5)});
            await unitOfWork.SaveChangeAsync();
        }

        /// <summary>
        /// Creates a <see cref="DataMessage"/> for logging purposes.
        /// </summary>
        /// <param name="username">The username associated with the request.</param>
        /// <param name="messageDigest">The path of the request or any message content.</param>
        /// <param name="messageType">The HTTP method used in the request.</param>
        /// <param name="ip">The IP address of the request source.</param>
        /// <returns>A <see cref="DataMessage"/> object containing the log information.</returns>
        private static DataMessage Message(string username, string messageDigest, string messageType, string ip)
        {
            return new DataMessage
            {
                UserName = username,
                MessageType = messageType,
                SourceIp = ip,
                SourceEntity = ConfigConstant.APP_NAME,
                InformationSystem = $"{Dns.GetHostName()} {Environment.OSVersion}",
                MessageContent = messageDigest,
                MessageDigest = MessageLogFactory.HashCode(messageDigest)
            };
        }
    }
}
