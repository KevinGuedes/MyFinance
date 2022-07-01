namespace MyFinance.Application.Generics.ApiService
{
    public class ApiErrorResponse
    {
        public string Title { get; private set; }
        public List<string> Errors { get; private set; }

        public ApiErrorResponse(string title, List<string> errors)
            => (Title, Errors) = (title, errors);
    }
}
