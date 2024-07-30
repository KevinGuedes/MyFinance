using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Contracts.User.Requests;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Commands.MagicSignIn;

public sealed class MagicSignInCommand(MagicSignInRequest request)
    : ICommand<UserInfoResponse>
{
    public string UrlSafeMagicSignInToken { get; init; } = request.UrlSafeMagicSignInToken;
}
