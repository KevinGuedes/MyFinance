using Microsoft.AspNetCore.Mvc;
using MyFinance.Contracts.Common;

namespace MyFinance.Contracts.User.Responses;

public sealed class TooManyFailedSignInAttemptsResponse(
    ProblemDetails problemDetails,
    DateTime lockoutEndOnUtc)
    : ProblemResponse(problemDetails)
{
    public DateTime LockoutEndOnUtc { get; init; } = lockoutEndOnUtc;
}
