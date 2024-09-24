using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Infrastructure.Extensions;

public static class HttpResponseExtensions
{
    public static Task WriteAsProblemPlusJsonAsync(
        this HttpResponse httpResponse,
        ObjectResult objectResult,
        CancellationToken cancellationToken = default)
    {
        return httpResponse.WriteAsJsonAsync(
               value: objectResult.Value,
               type: objectResult.Value!.GetType(),
               options: null,
               contentType: "application/problem+json",
               cancellationToken: cancellationToken);
    }
}
