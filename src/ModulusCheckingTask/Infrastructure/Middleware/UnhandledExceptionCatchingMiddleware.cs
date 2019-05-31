using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ModulusCheckingTask.App.Infrastructure.Middleware
{
    public class UnhandledExceptionCatchingMiddleware
    {
        #region Fields

        private readonly ILogger<UnhandledExceptionCatchingMiddleware> _logger;
        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public UnhandledExceptionCatchingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionCatchingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        #endregion

        #region Private Methods

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {       
            _logger.LogError(ex, $"An unhandled exception occurred. Trace Identifier: {context.TraceIdentifier}");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorPayload = new ValidationProblemDetails
            {
                Type = ex.GetType().Name,
                Detail = $"The following unhandled exception was occurred whilst processing a request: {ex.Message}",
                Status = context.Response.StatusCode,
                Extensions = {new KeyValuePair<string, object>("traceId", context.TraceIdentifier)},
                Title = "An exception occurred whilst processing a request."
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorPayload), Encoding.UTF8);
        }

        #endregion
    }
}
