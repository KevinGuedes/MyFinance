using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Application.UseCases.AccountTags.Commands.UpdateAccountTag;

public sealed class UpdateAccountTagValidator : AbstractValidator<UpdateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public UpdateAccountTagValidator(IAccountTagRepository accountTagRepository, ICurrentUserProvider currentUserProvider)
    {
        _accountTagRepository = accountTagRepository;
        _currentUserProvider = currentUserProvider;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 charactersmust have a maximum of 300 characters");

        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} invalid");

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 10).WithMessage("{PropertyName} must have between 3 and 10 characters")
            .MustAsync(async (command, tag, cancellationToken) =>
            {
                var currentUserId = _currentUserProvider.GetCurrentUserId();

                var existingBusinessUnit = await _accountTagRepository.GetByTagAsync(tag, currentUserId, cancellationToken);
                if (existingBusinessUnit is null)
                    return true;

                var isValidName = existingBusinessUnit.Id == command.Id;
                return isValidName;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}
