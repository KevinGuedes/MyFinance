namespace MyFinance.Application.Generics.ApiService
{
    public sealed class ErrorResponse
    {
        public string Title { get; private set; }
        public List<string> Errors { get; private set; }

        public ErrorResponse(string title, List<string> errors)
            => (Title, Errors) = (title, errors);

        public ErrorResponse(string title, string error)
            => (Title, Errors) = (title, new List<string> { error });
    }
}
