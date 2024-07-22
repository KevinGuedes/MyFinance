using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Queries;

public sealed record GetUserInfoQuery : IQuery<UserResponse>, IUserRequiredRequest
{
    public Guid CurrentUserId { get; set; }
}
