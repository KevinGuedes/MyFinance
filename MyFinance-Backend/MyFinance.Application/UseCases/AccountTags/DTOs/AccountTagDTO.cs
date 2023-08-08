using MyFinance.Application.Common.DTO;

namespace MyFinance.Application.UseCases.AccountTags.DTOs;

public class AccountTagDTO : BaseDTO
{
    public required string Tag { get; init; }
    public required string? Description { get; init; }
}
