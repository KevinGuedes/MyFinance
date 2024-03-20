using MyFinance.Application.Abstractions.RequestHandling;

namespace MyFinance.Application.Common.RequestHandling;

public abstract record UserBasedRequest : IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}