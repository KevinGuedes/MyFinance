using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.EmailSender;

internal sealed class EmailSenderOptions : IValidatableOptions
{
    [Required]
    public bool UseEmailConfirmation { get; set; }
}
