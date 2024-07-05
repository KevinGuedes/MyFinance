using FluentValidation;
using MyFinance.Application.Abstractions.Persistence.Repositories;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed class CreateAccountTagValidator : AbstractValidator<CreateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;

    public CreateAccountTagValidator(IAccountTagRepository accountTagRepository)
    {
        _accountTagRepository = accountTagRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(2, 5).WithMessage("{PropertyName} must have between 2 and 5 characters")
            .MustAsync(async (tag, cancellationToken) =>
            {
                var exists = await _accountTagRepository.ExistsByTagAsync(tag, cancellationToken);
                return !exists;
            }).WithMessage("The name '{PropertyValue}' has already been taken");
    }
}