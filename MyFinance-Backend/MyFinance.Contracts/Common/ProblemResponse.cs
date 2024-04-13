using Microsoft.AspNetCore.Mvc;

namespace MyFinance.Contracts.Common;

public class ProblemResponse : ProblemDetails
{
    public ProblemResponse(ProblemDetails problemDetails)
    {
        Status = problemDetails.Status;
        Title = problemDetails.Title;
        Detail = problemDetails.Detail;
        Instance = problemDetails.Instance;
        Type = problemDetails.Type;
        Extensions = problemDetails.Extensions;
    }
}