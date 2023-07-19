namespace MyFinance.Application.Common.ApiService;

public sealed class BadRequestResponse
{
    public string Title { get; private set; }
    public Dictionary<string, string[]> Errors { get; private set; }

    public BadRequestResponse(string title, Dictionary<string, string[]> errors)
        => (Title, Errors) = (title, errors);
}
