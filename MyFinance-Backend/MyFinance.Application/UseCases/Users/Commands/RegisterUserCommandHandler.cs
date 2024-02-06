using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using MyFinance.Application.Common.RequestHandling.Commands;
using MyFinance.Application.UseCases.Transfers.Commands.RegisterTransfer;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Application.UseCases.Users.Commands;
internal sealed class RegisterUserCommandHandler(
    ILogger<RegisterUserCommandHandler> logger, 
    IUserRepository userRepository) : ICommandHandler<RegisterUserCommand, User>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;

    public Task<Result<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new User");
        var user = new User(request.Name, request.Email, request.PlainTextPassword);
        _userRepository.Insert(user);
        _logger.LogInformation("User successfully registered");

        return Task.FromResult(Result.Ok(user));
    }
}
