using MediatR;
using Microsoft.Extensions.Logging;
using SpeedyWheelsSales.Application.Interfaces;

namespace SpeedyWheelsSales.Application.Core.Behaviors;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly ILogger<TRequest> _logger;

    public RequestLoggingPipelineBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation(
            "Processing request {RequestName}", requestName);

        TResponse result = await next();


        if (result is IResult typedResult)
        {
            if (typedResult.IsSuccess)
            {
                _logger.LogInformation(
                    "Completed request {RequestName}", requestName);
            }
            else
            {
                if (!string.IsNullOrEmpty(typedResult.Error))
                {
                    _logger.LogError(
                        "Completed request {RequestName} with error: {Error}", requestName,
                        typedResult.Error);
                }
                else if (typedResult.ValidationErrors.Any())
                {
                    _logger.LogError(
                        "Completed request {RequestName} with validation errors:", requestName);

                    foreach (var validationError in typedResult.ValidationErrors)
                    {
                        _logger.LogError(
                            "{PropertyName}: {ErrorMessage}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                else if (typedResult.IsEmpty)
                {
                    _logger.LogWarning(
                        "Completed request {RequestName} with empty result", requestName);
                }
            }
        }
        else
        {
            _logger.LogError(
                "RequestLoggingPipelineBehavior: Unexpected Result type encountered for request {RequestName}.",
                requestName);
        }

        return result;
    }
}