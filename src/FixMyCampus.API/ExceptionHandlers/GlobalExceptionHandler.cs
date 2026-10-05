using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FixMyCampus.API.ExceptionHandlers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct
    )
    {
        var (
            status,
            title,
            detail,
            errors
        ) = exception switch
        {
            ValidationException ve => (
                StatusCodes.Status400BadRequest,
                "Validation failed",
                "One or more fields are invalid. See errors for details.",
                (IDictionary<string, string[]>?)
                    ve.Errors
                        .GroupBy(
                            e => e.PropertyName
                        )
                        .ToDictionary(
                            g => g.Key,
                            g => g
                                .Select(
                                    e => e.ErrorMessage
                                )
                                .ToArray()
                        )
            ),

            FixMyCampus.Domain.Exceptions.DomainException de => (
                StatusCodes.Status400BadRequest,
                "Business Rule Violation",
                de.Message,
                null
            ),

            FixMyCampus.Application.Common.Exceptions.BusinessRuleException bre => (
                StatusCodes.Status400BadRequest,
                "Business Rule Violation",
                bre.Message,
                null
            ),

            FixMyCampus.Application.Common.Exceptions.NotFoundException nfe => (
                StatusCodes.Status404NotFound,
                "Not Found",
                nfe.Message,
                null
            ),

            FixMyCampus.Application.Common.Exceptions.ConflictException ce => (
                StatusCodes.Status409Conflict,
                "Conflict",
                ce.Message,
                null
            ),

            FixMyCampus.Application.Common.Exceptions.UnauthorizedException ue => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                ue.Message,
                null
            ),

            FixMyCampus.Application.Common.Exceptions.ForbiddenException fe => (
                StatusCodes.Status403Forbidden,
                "Forbidden",
                fe.Message,
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Server error",
                $"An unexpected error occurred. Trace ID: {httpContext.TraceIdentifier}",
                null
            )
        };

        if (
            status
            ==
            StatusCodes.Status500InternalServerError
        )
        {
            logger.LogError(
                exception,
                "Unhandled exception (trace={TraceId})",
                httpContext.TraceIdentifier
            );
        }

        var problem =
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance =
                    httpContext.Request.Path
            };

        if (errors is not null)
        {
            problem.Extensions["errors"] =
                errors;
        }

        httpContext.Response.StatusCode =
            status;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response
            .WriteAsJsonAsync(
                problem,
                ct
            );

        return true;
    }
}