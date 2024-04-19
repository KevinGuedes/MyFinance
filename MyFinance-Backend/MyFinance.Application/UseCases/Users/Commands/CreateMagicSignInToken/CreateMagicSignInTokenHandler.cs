using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.CreateMagicSignInToken;

internal sealed class CreateMagicSignInTokenHandler(
    ISignInManager signInManager,
    IUserRepository userRepository) 
    : ICommandHandler<CreateMagicSignInTokenCommand>
{
    private readonly ISignInManager _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(CreateMagicSignInTokenCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);

        //To avoid giving information to hackers
        if (user is null)
            return Result.Ok();

        var magicSignInId = Guid.NewGuid();
        user.SetMagicSignInId(magicSignInId);
        var token = _signInManager.CreateMagicSignInToken(magicSignInId);

        //send token via email

        _userRepository.Update(user);

        return Result.Ok();
    }
}
