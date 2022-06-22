using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyFinance.Presentation.Middlewares
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
            => (_logger, _next) = (logger, next);

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (title, status) = GetResponseInfoAccordingToException(exception);
            var response = BuildResponseObject(title, status, exception);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = status;

            var serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, serializerOptions));
        }

        private static (string, int) GetResponseInfoAccordingToException(Exception exception)
            => exception switch
            {
                InvalidRequestException invalidRequestException
                    => (invalidRequestException.Title, StatusCodes.Status422UnprocessableEntity),
                _
                    => ("Unexpected server behavior", StatusCodes.Status500InternalServerError)
            };

        private static object BuildResponseObject(string title, int status, Exception exception)
        {
            var errors = GetErrors(exception);
            var message = exception.Message;
            return new { title, status, message, errors };
        }

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            if (exception is InvalidRequestException invalidRequestException)
                return new Dictionary<string, string[]>(invalidRequestException.Errors);

            return null;
        }
    }
}
