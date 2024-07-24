using FluentResults;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;

namespace MyFinance.Application.UseCases.Users.Commands.ConfirmRegistration;

internal sealed class ConfirmRegistrationHandler(
    ITokenProvider tokenProvider,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<ConfirmRegistrationCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(ConfirmRegistrationCommand command, CancellationToken cancellationToken)
    {
        var isValidToken = _tokenProvider.TryGetUserIdFromUrlSafeConfirmRegistrationToken(
            command.UrlSafeConfirmRegistrationToken,
            out var userId);

        if (!isValidToken)
            return HandleInvalidConfirmRegistrationToken();

        var user = await _myFinanceDbContext.Users.FindAsync([userId], cancellationToken);

        if (user is null)
            return HandleInvalidConfirmRegistrationToken();

        if (user.IsEmailVerified)
            return Result.Ok();

        user.VerifyEmail();
        _myFinanceDbContext.Users.Update(user);

        return Result.Ok();
    }

    private static Result HandleInvalidConfirmRegistrationToken()
        => Result.Fail(new InternalServerError("Invalid confirm registration token"));
}
