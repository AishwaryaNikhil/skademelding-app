using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TelenorSkademeldingApp.Application.Common.Exceptions;

namespace TelenorSkademeldingApp.Api.ErrorHandling;

/// <summary>Translates domain validation failures into RFC 7807 problem responses.</summary>
public sealed class ClaimValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ClaimValidationException validationException)
        {
            return false;
        }

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Valideringsfeil",
            Detail = validationException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}
