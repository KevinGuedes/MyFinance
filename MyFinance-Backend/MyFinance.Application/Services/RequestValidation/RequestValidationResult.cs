namespace MyFinance.Application.Services.RequestValidation
{
    public class RequestValidationResult
    {
        public Dictionary<string, string[]>? Errors { get; private set; }
        public bool IsSuccess { get; private set; }

        public RequestValidationResult(Dictionary<string, string[]> errors)
            => (IsSuccess, Errors) = (false, errors);

        public RequestValidationResult(bool isSuccess)
            => (IsSuccess, Errors) = (isSuccess, null);

        public void Deconstruct(out bool isSuccess, out Dictionary<string, string[]>? errors)
            => (isSuccess, errors) = (IsSuccess, Errors);
    }
}
