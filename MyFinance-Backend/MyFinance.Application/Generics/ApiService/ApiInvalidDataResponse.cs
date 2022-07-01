namespace MyFinance.Application.Generics.ApiService
{
    public class ApiInvalidDataResponse
    {
        public string Title { get; private set; }
        public Dictionary<string, string[]> Errors { get; private set; }

        public ApiInvalidDataResponse(string title, Dictionary<string, string[]> errors)
            => (Title, Errors) = (title, errors);
    }
}
