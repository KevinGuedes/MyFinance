using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.Users.DTOs;

public sealed class UserDTO : EntityDTO
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}