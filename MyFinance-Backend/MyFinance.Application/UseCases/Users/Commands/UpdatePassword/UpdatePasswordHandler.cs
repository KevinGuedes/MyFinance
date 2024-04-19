using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.UpdatePassword;

internal sealed class UpdatePasswordHandler(
    IPasswordManager passwordManager,
    IUserRepository userRepository) 
    : ICommandHandler<UpdatePasswordCommand>
{
    private readonly IPasswordManager _passwordManager = passwordManager;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result> Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.CurrentUserId, cancellationToken);

        if(user is null)
            return Result.Fail(new InternalServerError());

        var isPasswordValid = _passwordManager.VerifyPassword(
            command.PlainTextCurrentPassword, 
            user.PasswordHash);

        if (!isPasswordValid)
        {
            var badRequestError = new BadRequestError(
                nameof(command.PlainTextCurrentPassword), 
                "Password is not valid");

            return Result.Fail(badRequestError);
        }

        var arePasswordsSimilar = _passwordManager.ArePasswordsSimilar(
            command.PlainTextCurrentPassword, 
            command.PlainTextNewPassword);

        if(arePasswordsSimilar)
        {
            var message = "The new password must not be similar to the current password";
            var unprocessableEntityError = new UnprocessableEntityError(message);
            return Result.Fail(unprocessableEntityError);
        }

        var newPasswordHash = _passwordManager.HashPassword(command.PlainTextNewPassword);
        user.UpdatePasswordHash(newPasswordHash);

        return Result.Ok();
    }
}
