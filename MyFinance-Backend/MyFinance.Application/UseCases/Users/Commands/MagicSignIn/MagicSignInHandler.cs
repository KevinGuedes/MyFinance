using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

internal sealed class MagicSignInHandler(
    ISignInManager signInManager,
    IPasswordManager passwordManager,
    IUserRepository userRepository) 
    : ICommandHandler<MagicSignInCommand, SignInResponse>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<SignInResponse>> Handle(MagicSignInCommand command, CancellationToken cancellationToken)
    {
        if (!_signInManager.TryGetMagicSignInIdFromToken(command.Token, out var magicSignInId))
            return HandleInvalidToken();

        var user = await _userRepository.GetByMagicSignInIdAsync(magicSignInId, cancellationToken);

        if(user is null)
            return HandleInvalidToken();

        user.ResetLockout();
        user.ResetMagicSignInId();
        _userRepository.Update(user);

        await _signInManager.SignInAsync(user);
        var shouldUpdatePassword = _passwordManager.ShouldUpdatePassword(user.LastPasswordUpdateOnUtc);
        
        return Result.Ok(new SignInResponse(shouldUpdatePassword));
    }

    private static Result HandleInvalidToken()
        => Result.Fail(new UnauthorizedError("Invalid sign in token"));
}
