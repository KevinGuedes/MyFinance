using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Infrastructure.Extensions;

public static class HttpResponseExtension
{
    public static async Task WriteProblemResponseAsJsonAsync(this HttpResponse httpResponse, ObjectResult problemResponse)
    {
        await httpResponse.WriteAsJsonAsync(
               value: problemResponse.Value,
               type: problemResponse.Value!.GetType(),
               options: null,
               contentType: "application/problem+json");
    }
}
