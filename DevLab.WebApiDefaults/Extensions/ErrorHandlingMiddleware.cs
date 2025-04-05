using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace DevLab.WebApiDefaults.Extensions;

public class ErrorHandlingMiddleware
{
    private readonly IWebHostEnvironment _env;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var problemDetails = new ValidationProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Ocorreram erros de validação",
            Status = (int)HttpStatusCode.BadRequest,
            Instance = context.Request.Path,
            Detail = "Verifique os detalhes para mais informações sobre os erros"
        };

        foreach (var error in ex.Errors)
            if (!problemDetails.Errors.ContainsKey(error.PropertyName))
            {
                problemDetails.Errors[error.PropertyName] = [error.ErrorMessage];
            }
            else
            {
                var errorMessages = problemDetails.Errors[error.PropertyName].ToList();
                errorMessages.Add(error.ErrorMessage);
                problemDetails.Errors[error.PropertyName] = errorMessages.ToArray();
            }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        await context.Response.WriteAsync(json);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Ocorreu um erro interno no servidor",
            Status = (int)HttpStatusCode.InternalServerError,
            Instance = context.Request.Path,
            Detail = _env.IsDevelopment() ? ex.Message : "Um erro inesperado ocorreu."
        };

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
            problemDetails.Extensions["stackTrace"] = ex.StackTrace;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(problemDetails, options);
        await context.Response.WriteAsync(json);
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}