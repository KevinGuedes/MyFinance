namespace MyFinance.Application.Abstractions.RequestHandling;

internal interface IUserRequiredRequest
{
    public Guid CurrentUserId { get; set; }
}
