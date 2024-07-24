using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.ResendConfirmRegistrationEmail;

internal sealed class ResendConfirmRegistrationEmailHandler(
    ITokenProvider tokenProvider,
    IEmailSender emailSender,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<ResendConfirmRegistrationEmailCommand>
{
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(ResendConfirmRegistrationEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users
            .Where(user => user.Email == command.Email)
            .Select(user => new { user.Id, user.Email })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Ok();

        var urlSafeConfirmRegistrationToken
            = _tokenProvider.CreateUrlSafeConfirmRegistrationToken(user.Id);

        await _emailSender.SendConfirmRegistrationEmailAsync(
            user.Email,
            urlSafeConfirmRegistrationToken,
            cancellationToken);

        return Result.Ok();
    }
}
