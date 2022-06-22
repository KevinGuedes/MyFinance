namespace MyFinance.Application.Exceptions
{
    public class BaseException: Exception
    {
        public BaseException(string title, string message) : base(message)
            => Title = title;

        public string Title { get; private set; }
    }
}
