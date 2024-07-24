using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Queries;

internal sealed class GetUserInfoHandler(
    IMyFinanceDbContext myFinanceDbContext,
    IPasswordManager passwordManager)
    : IQueryHandler<GetUserInfoQuery, UserResponse>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly IPasswordManager _passwordManager = passwordManager;

    public async Task<Result<UserResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users
            .FindAsync([request.CurrentUserId], cancellationToken);

        if (user is null)
            return Result.Fail(new InternalServerError("Unable to identify user"));

        return Result.Ok(UserMapper.DTR.Map(user, _passwordManager.ShouldUpdatePassword(user)));
    }
}
