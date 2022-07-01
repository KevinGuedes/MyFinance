namespace MyFinance.Application.Generics.ApiService
{
    public class ApiInvalidDataResponse
    {
        public int Status { get; private set; }
        public string Title { get; private set; }
        public Dictionary<string, string[]> Errors { get; private set; }

        public ApiInvalidDataResponse(int status, string title, Dictionary<string, string[]> errors)
            => (Status, Title, Errors) = (status, title, errors);
    }
}
