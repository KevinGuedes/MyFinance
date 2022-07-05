namespace MyFinance.Application.Generics.ApiService
{
    public sealed class InvalidRequestResponse
    {
        public string Title { get; private set; }
        public Dictionary<string, string[]> Errors { get; private set; }

        public InvalidRequestResponse(string title, Dictionary<string, string[]> errors)
            => (Title, Errors) = (title, errors);
    }
}
