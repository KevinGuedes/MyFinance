namespace MyFinance.Application.Abstractions.RequestHandling;

public interface IUserBasedRequest
{
    public Guid CurrentUserId { get; set; }
}