using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.SignOutFromAllDevices;

internal sealed class SignOutFromAllDevicesHandler(
    ISignInManager signInManager,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<SignOutFromAllDevicesCommand>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(SignOutFromAllDevicesCommand command, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users.FindAsync([command.CurrentUserId], cancellationToken);

        if (user is null)
            return Result.Fail(new InternalServerError("Unable to identify user"));

        await _signInManager.SignOutAsync();
        user.UpdateSecurityStamp();
        _myFinanceDbContext.Users.Update(user);

        return Result.Ok();
    }
}
