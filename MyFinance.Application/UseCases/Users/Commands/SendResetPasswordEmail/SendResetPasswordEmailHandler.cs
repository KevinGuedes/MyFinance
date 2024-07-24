using FluentResults;
using Microsoft.EntityFrameworkCore;
using MyFinance.Application.Abstractions.Persistence;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.Users.Commands.SendResetPasswordEmail;

internal sealed class SendResetPasswordEmailHandler(
    ITokenProvider tokenProvider,
    IEmailSender emailSender,
    IMyFinanceDbContext myFinanceDbContext)
    : ICommandHandler<SendResetPasswordEmailCommand>
{
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IMyFinanceDbContext _myFinanceDbContext = myFinanceDbContext;

    public async Task<Result> Handle(SendResetPasswordEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _myFinanceDbContext.Users
            .FirstOrDefaultAsync(user => user.Email == command.Email, cancellationToken);

        if (user is null)
            return Result.Ok();

        if (!user.IsEmailVerified)
            return Result.Ok();

        var urlSafeResetPasswordToken = _tokenProvider.CreateUrlSafeResetPasswordToken(user.Id);

        await _emailSender.SendResetPasswordEmailAsync(
            user.Email,
            urlSafeResetPasswordToken,
            cancellationToken);

        return Result.Ok();
    }
}
