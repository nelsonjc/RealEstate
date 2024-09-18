using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Exceptions;
using System.Globalization;

namespace RealEstate.Infraestructure.Filter
{
    /// <summary>
    /// Class to global exception 
    /// </summary>
    public class GlobalExepctionFilter : IExceptionFilter
    {
        /// <summary>
        /// Variable to do logger
        /// </summary>
        private readonly ILogger<GlobalExepctionFilter> _logger;

        /// <summary>
        /// Ctor to global exception
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExepctionFilter(ILogger<GlobalExepctionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method to controller exception
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.Exception is BusinessException exception)
            {
                var status = (int)exception.Status;
                var response = new Response() { Status = status, Message = exception.Message, Description =  exception.Description };

                context.Result = new ObjectResult(response);
                context.HttpContext.Response.StatusCode = status;
                context.ExceptionHandled = true;

                RegisterLogInformation(exception);
            }
            else
            {
                var response = new Response()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = MessageConstant.DEFAULT_ERROR_MESSAGE,
                    Description = MessageConstant.DEFAULT_ERROR_MESSAGE,
                };

                context.Result = new ObjectResult(response);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.ExceptionHandled = true;

                RegisterLogError(context.Exception);
            }
        }

        /// <summary>
        /// Method to register log
        /// </summary>
        /// <param name="ex">Parameter with exception message</param>
        private void RegisterLogInformation(Exception ex) => _logger.LogInformation(MessageConstant.LOG_ERROR_MESSAGE, ex.Message);

        /// <summary>
        /// Method to register log error detail
        /// </summary>
        /// <param name="ex">Parameter with exception message</param>
        private void RegisterLogError(Exception ex)
        {
            _logger.LogError(
                MessageConstant.LOG_ERROR_DETAIL_MESSAGE,
                ex.Message,
                ex.InnerException?.Message ?? string.Empty,
                DateTime.UtcNow.AddHours(-5).ToString(CultureInfo.InvariantCulture)
            );
        }
    }
}
