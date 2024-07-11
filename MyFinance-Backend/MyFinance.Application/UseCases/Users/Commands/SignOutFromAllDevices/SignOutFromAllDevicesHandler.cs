using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.SignOutFromAllDevices;

internal sealed class SignOutFromAllDevicesHandler(
    ISignInManager signInManager,
    IUserRepository userRepository)
    : ICommandHandler<SignOutFromAllDevicesCommand>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(SignOutFromAllDevicesCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.CurrentUserId, cancellationToken);

        if (user is null)
            return Result.Fail(new InternalServerError("Unable to identify user"));

        await _signInManager.SignOutAsync();
        user.UpdateSecurityStamp();
        _userRepository.Update(user);

        return Result.Ok();
    }
}
