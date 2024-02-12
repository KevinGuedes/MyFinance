using FluentValidation;
using MyFinance.Application.Services.CurrentUserProvider;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.UseCases.AccountTags.Commands.CreateAccountTag;

public sealed class CreateAccountTagValidator : AbstractValidator<CreateAccountTagCommand>
{
    private readonly IAccountTagRepository _accountTagRepository;
    private readonly ICurrentUserProvider _currentUserProvider;

    public CreateAccountTagValidator(IAccountTagRepository accountTagRepository, ICurrentUserProvider currentUserProvider)
    {
        _accountTagRepository = accountTagRepository;
        _currentUserProvider = currentUserProvider;
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
                var currentUserId = _currentUserProvider.GetCurrentUserId();
                var exists = await _accountTagRepository.ExistsByTagAsync(tag, currentUserId, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}
