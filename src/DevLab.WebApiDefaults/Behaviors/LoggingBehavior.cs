﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace DevLab.WebApiDefaults.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Iniciando comando {RequestName} {@Request}", requestName, request);

        try
        {
            var response = await next();
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar comando {RequestName} {@Request}", requestName, request);
            throw;
        }
    }
}