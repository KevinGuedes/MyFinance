using MyFinance.Infrastructure.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Infrastructure.Services.Storage;

public class StorageOptions : IValidatableOptions
{
    [Required]
    public string UserImagesContainerName { get; set; } = string.Empty;
}
