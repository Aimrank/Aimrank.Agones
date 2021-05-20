using Aimrank.Agones.Infrastructure.CSGO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace Aimrank.Agones.Api.Middleware
{
    public class ExceptionsHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionsHandlerMiddleware> _logger;

        public ExceptionsHandlerMiddleware(ILogger<ExceptionsHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception exception)
            {
                var response = MapExceptionToProblemDetails(exception);
                if (response.Status == StatusCodes.Status500InternalServerError)
                {
                    _logger.LogError(exception, exception.Message);
                }

                context.Response.StatusCode = response.Status ?? 500;
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static ProblemDetails MapExceptionToProblemDetails(Exception exception)
            => exception switch
            {
                ServerException ex => Problem(ex.Message, StatusCodes.Status409Conflict),
                _ => Problem("Internal server error", StatusCodes.Status500InternalServerError)
            };

        private static ProblemDetails Problem(string title, int status)
            => new ProblemDetails
            {
                Title = title,
                Status = status
            };
    }
}