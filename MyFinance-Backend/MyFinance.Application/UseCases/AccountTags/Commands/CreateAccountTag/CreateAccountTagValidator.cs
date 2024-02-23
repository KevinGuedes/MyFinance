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

        RuleFor(command => command.Description)
            .MaximumLength(300).WithMessage("{PropertyName} must have a maximum of 300 characters");

        RuleFor(command => command.Tag)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} must not be null")
            .NotEmpty().WithMessage("{PropertyName} must not be empty")
            .Length(3, 10).WithMessage("{PropertyName} must have between 3 and 10 characters")
            .MustAsync(async (tag, cancellationToken) =>
            {
                var currentUserId = _currentUserProvider.GetCurrentUserId();
                var exists = await _accountTagRepository.ExistsByTagAsync(tag, currentUserId, cancellationToken);
                return !exists;
            }).WithMessage("This {PropertyName} has already been taken");
    }
}
