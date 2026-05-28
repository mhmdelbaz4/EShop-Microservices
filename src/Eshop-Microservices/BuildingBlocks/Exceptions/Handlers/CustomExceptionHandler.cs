
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BuildingBlocks.Exceptions.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                          Exception exception,
                                          CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = exception switch
        {
            ValidationException => new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title  = "Validation Failed"    
            },
            NotFoundException => new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Resource Not Found"
            },
            InternalServerException => new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Internal Server Error"
            },
            _ => new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError
            }
        };

        problemDetails.Type = exception.GetType().Name;
        problemDetails.Instance = httpContext.Request.Path;
        if (exception is ValidationException validationException)
        {
            var errors = validationException.Errors
                                        .GroupBy(e => e.PropertyName)
                                        .Select(g => new
                                        {
                                            PropertyName = g.Key,
                                            ErrorMessages = g.Select(x => x.ErrorMessage)
                                        });
            problemDetails.Extensions.Add("Errors", errors);
        }


        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
