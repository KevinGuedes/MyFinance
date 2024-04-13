using MyFinance.Domain.Entities;

namespace MyFinance.Application.Abstractions.Services;

public interface IUserManager
{
    bool IsLockedOut(User user);
}
