using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SendMagicSignInEmail;

internal sealed class SendMagicSignInEmailHandler(
    ITokenProvider tokenProvider,
    IEmailSender emailSender,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<SendMagicSignInEmailCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task<Result> Handle(SendMagicSignInEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users
            .Where(user => user.Email == command.Email)
            .Select(user => new { user.Id, user.Email, user.IsEmailVerified })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Ok();

        if (!user.IsEmailVerified)
            return Result.Ok();

        var urlSafeMagicSignInToken = _tokenProvider.CreateUrlSafeMagicSignInToken(user.Id);

        await _emailSender.SendMagicSignInEmailAsync(
            user.Email,
            urlSafeMagicSignInToken,
            cancellationToken);

        return Result.Ok();
    }
}
