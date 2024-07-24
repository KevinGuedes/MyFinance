using MyFinance.Application.Abstractions.RequestHandling;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Queries.GetUserInfo;

public sealed record GetUserInfoQuery : IQuery<UserInfoResponse>, IUserRequiredRequest
{
    public Guid CurrentUserId { get; set; }
}
