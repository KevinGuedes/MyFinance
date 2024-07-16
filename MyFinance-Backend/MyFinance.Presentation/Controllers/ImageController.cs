using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Abstractions.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[SwaggerTag("Image management")]
public class ImageController(IStorage storage) : ControllerBase
{
    private readonly IStorage _storage = storage;

    [HttpPost]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Uploads an image")]
    public async Task<IActionResult> UploadImageAsync(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        await _storage.UploadUserImage(file.FileName, stream, cancellationToken);
        return Ok();
    }
}
