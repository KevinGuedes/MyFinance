using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Queries.GetUserInfo;

internal sealed class GetUserInfoHandler(
    IMyFinanceDbContext myFinanceDbContext,
    IPasswordManager passwordManager)
    : IQueryHandler<GetUserInfoQuery, UserInfoResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly IPasswordManager _passwordManager = passwordManager;

    public async Task<Result<UserInfoResponse>> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == query.CurrentUserId)
            .Select(user => new
            {
                user.Name,
                user.CreatedOnUtc,
                user.LastPasswordUpdateOnUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Fail(new InternalServerError("Unable to identify user"));

        var shouldUpdatePassword = _passwordManager.ShouldUpdatePassword(
            user.CreatedOnUtc,
            user.LastPasswordUpdateOnUtc);

        return new UserInfoResponse
        {
            Name = user.Name,
            ShouldUpdatePassword = shouldUpdatePassword,
        };
    }
}
