using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using MyFinance.Application.Abstractions.Services;

namespace MyFinance.Infrastructure.Services.Storage;

public class Storage(
    BlobServiceClient blobClient, 
    IOptions<StorageOptions> storageOptions) 
    : IStorage
{
    private readonly BlobServiceClient _blobClient = blobClient;
    private readonly StorageOptions _storageOptions = storageOptions.Value;

    public async Task UploadUserImage(string blobName, Stream stream, CancellationToken cancellationToken)
    {
        var b = _blobClient
             .GetBlobContainerClient(_storageOptions.UserImagesContainerName)
             .GetBlobClient(blobName);

        await b.UploadAsync(stream, cancellationToken);

        var url = b.Uri.AbsoluteUri;

        var x = 1;
    }
}
