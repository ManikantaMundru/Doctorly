using Doctorly.Calendar.Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Doctorly.Calendar.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(exception,
                    "Cannot handle exception because the response has already started. TraceId: {TraceId}",
                    context.TraceIdentifier);
                throw exception;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            var (statusCode, response, logLevel) = exception switch
            {
                ValidationException validationException => (
                    StatusCodes.Status400BadRequest,
                    new ApiErrorResponse(
                        "validation_error",
                        "One or more validation errors occurred.",
                        context.TraceIdentifier,
                        validationException.Errors
                            .Select(x => new ApiValidationError(x.PropertyName, x.ErrorMessage))
                            .ToList()),
                    LogLevel.Warning),

                DomainException => (
                    StatusCodes.Status400BadRequest,
                    new ApiErrorResponse(
                        "business_rule_violation",
                        "The request could not be processed.",
                        context.TraceIdentifier),
                    LogLevel.Warning),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    new ApiErrorResponse(
                        "not_found",
                        "The requested resource was not found.",
                        context.TraceIdentifier),
                    LogLevel.Information),

                DbUpdateConcurrencyException => (
                    StatusCodes.Status409Conflict,
                    new ApiErrorResponse(
                        "concurrency_conflict",
                        "The resource was modified by another request. Please reload and try again.",
                        context.TraceIdentifier),
                    LogLevel.Warning),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    new ApiErrorResponse(
                        "server_error",
                        "An unexpected error occurred.",
                        context.TraceIdentifier),
                    LogLevel.Error)
            };

            context.Response.StatusCode = statusCode;

            LogException(exception, logLevel, context.TraceIdentifier);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }

        private void LogException(Exception exception, LogLevel logLevel, string traceId)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    _logger.LogInformation(exception,
                        "Request failed with a handled exception. TraceId: {TraceId}",
                        traceId);
                    break;

                case LogLevel.Warning:
                    _logger.LogWarning(exception,
                        "Request failed with a handled exception. TraceId: {TraceId}",
                        traceId);
                    break;

                default:
                    _logger.LogError(exception,
                        "Unhandled exception occurred. TraceId: {TraceId}",
                        traceId);
                    break;
            }
        }

        private sealed record ApiErrorResponse(
            string Code,
            string Message,
            string TraceId,
            IReadOnlyList<ApiValidationError> Errors = null);

        private sealed record ApiValidationError(
            string Property,
            string Message);
    }
}