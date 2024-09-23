using MyFinance.Domain.Entities;

namespace MyFinance.TestCommon.Factories;

public static class UserFactory
{
    //password = plainTextPassword123456#%
    public static User CreateUser(
        string name = "Test User",
        string email = "test-user@test.com",
        string passwordHash = "$2a$12$BYcm.gY761I81JaGU0AXYe0ltLzRC6okZKw78MDV6EjsoaKZuo4zi")
        => new(name, email, passwordHash);
}
