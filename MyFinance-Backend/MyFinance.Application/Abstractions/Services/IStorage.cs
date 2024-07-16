namespace MyFinance.Application.Abstractions.Services;

public interface IStorage
{
    Task UploadUserImage(string blobName, Stream stream, CancellationToken cancellationToken);
}
