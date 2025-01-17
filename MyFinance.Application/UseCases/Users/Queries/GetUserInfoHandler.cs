﻿using FluentResults;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Abstractions.Services;
using MyFinance.Application.Common.Errors;
using MyFinance.Application.Mappers;
using MyFinance.Contracts.User.Responses;

namespace MyFinance.Application.UseCases.Users.Queries;

internal sealed class GetUserInfoHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager)
    : IQueryHandler<GetUserInfoQuery, UserResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordManager _passwordManager = passwordManager;

    public async Task<Result<UserResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.CurrentUserId, cancellationToken);

        if (user is null)
            return Result.Fail(new InternalServerError("Unable to identify user"));

        return Result.Ok(UserMapper.DTR.Map(user, _passwordManager.ShouldUpdatePassword(user)));
    }
}
