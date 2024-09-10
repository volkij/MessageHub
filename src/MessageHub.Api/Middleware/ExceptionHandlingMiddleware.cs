using MessageHub.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageHub.Api.Middleware
{
    internal class ExceptionHandlingMiddleware
    {
        private const string UnhandledExceptionMsg = "An unhandled exception has occurred while executing the request.";

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception) when (context.RequestAborted.IsCancellationRequested)
            {
                const string message = "Request was cancelled";
                _logger.LogDebug(exception, message);

                context.Response.Clear();
                context.Response.StatusCode = 499; //Client Closed Request
            }
            catch (NotFoundException notFoundException)
            {
                _logger.LogWarning(notFoundException, "Resource not found: {Message}", notFoundException.Message);

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status404NotFound;

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = notFoundException.Message
                };
                var json = ToJson(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception is MessageHubException ? exception.Message : UnhandledExceptionMsg);

                const string contentType = "application/problem+json";
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = contentType;

                var problemDetails = CreateProblemDetails(context, exception);
                var json = ToJson(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }

        private ProblemDetails CreateProblemDetails(in HttpContext context, in Exception exception)
        {
            var statusCode = context.Response.StatusCode;
            var reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode);
            if (string.IsNullOrEmpty(reasonPhrase))
            {
                reasonPhrase = UnhandledExceptionMsg;
            }

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = reasonPhrase,
            };

            problemDetails.Detail = exception.ToString();
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            problemDetails.Extensions["data"] = exception.Data;

            return problemDetails;
        }

        private string ToJson(in ProblemDetails problemDetails)
        {
            try
            {
                return JsonSerializer.Serialize(problemDetails, SerializerOptions);
            }
            catch (Exception ex)
            {
                const string msg = "An exception has occurred while serializing error to JSON";
                _logger.LogError(ex, msg);
            }

            return string.Empty;
        }
    }
}
