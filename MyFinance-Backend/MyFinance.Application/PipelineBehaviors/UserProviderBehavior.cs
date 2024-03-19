using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.PipelineBehaviors;

public sealed class UserProviderBehavior<TRequest>(
    ILogger<UserProviderBehavior<TRequest>> logger, 
    ICurrentUserProvider currentUserProvider) 
    : IRequestPreProcessor<TRequest> where TRequest : IUserBasedRequest
{
    private readonly ILogger<UserProviderBehavior<TRequest>> _logger = logger;
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving User Id");
        request.CurrentUserId = _currentUserProvider.GetCurrentUserId();
        return Task.CompletedTask;
    }
}
