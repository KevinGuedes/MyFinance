using FluentValidation;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed class CreateAccountTagValidator : AbstractValidator<CreateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;

    public CreateAccountTagValidator(IAccountTagRepository accountTagRepository)
    {
        _accountTagRepository = accountTagRepository;
        ClassLevelCascadeMode = CascadeMode.Stop;

        When(command => command.Description is not null, () =>
        {
            RuleFor(command => command.Description)
                .NotNull().WithMessage("{PropertyName} must not be null")
                .NotEmpty().WithMessage("{PropertyName} must not be empty")
                .Length(10, 200).WithMessage("{PropertyName} must have between 10 and 200 characters");
        });
            
        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(2, 6).WithMessage("{PropertyName} must have between 2 and 6 characters")
            .MustAsync(async (tag, cancellationToken) =>
            {
                var exists = await _accountTagRepository.ExistsByTagAsync(tag, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}
