using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Infrastructure.Extensions;

public static class HttpResponseExtension
{
    public static async Task WriteAsProblemPlusJsonAsync(this HttpResponse httpResponse, ObjectResult objectResult)
    {
        await httpResponse.WriteAsJsonAsync(
               value: objectResult.Value,
               type: objectResult.Value!.GetType(),
               options: null,
               contentType: "application/problem+json");
    }

    public static async Task WriteAsProblemPlusJsonAsync(
        this HttpResponse httpResponse, 
        ObjectResult objectResult,
        CancellationToken cancellationToken)
    {
        await httpResponse.WriteAsJsonAsync(
               value: objectResult.Value,
               type: objectResult.Value!.GetType(),
               options: null,
               contentType: "application/problem+json",
               cancellationToken: cancellationToken);
    }
}
