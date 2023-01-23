namespace MyFinance.Application.Generics.ApiService
{
    public sealed class InternalServerErrorResponse
    {
        public string Title { get; private set; }
        public List<string> Errors { get; private set; }

        public InternalServerErrorResponse(string title, List<string> errors)
            => (Title, Errors) = (title, errors);

        public InternalServerErrorResponse(string title, string error)
            => (Title, Errors) = (title, new List<string> { error });
    }
}
