namespace MyFinance.Application.Generics.ApiService
{
    public class ApiErrorResponse
    {
        public int Status { get; private set; }
        public string Title { get; private set; }
        public List<string> Errors { get; private set; }

        public ApiErrorResponse(int status, string title, List<string> errors)
            => (Status, Title, Errors) = (status, title, errors);
    }
}
