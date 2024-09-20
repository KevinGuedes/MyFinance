using MyFinance.ArchitectureTests.Common;

namespace MyFinance.ArchitectureTests.Contracts;

public class ContractsTest : BaseArchitectureTest
{
   private const string RequestsSuffix = "Request";
   private const string ResponsesSuffix = "Response";

    [Fact]
    public void Responses_Should_HaveResponsesSuffix()
    {
        var result = Types.InAssembly(ContractsAssembly)
            .That()
            .ResideInNamespaceEndingWith(ResponsesSuffix)
            .Should()
            .HaveNameEndingWith(ResponsesSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Requests_Should_HaveRequestsSuffix()
    {
        var result = Types.InAssembly(ContractsAssembly)
            .That()
            .ResideInNamespaceEndingWith(RequestsSuffix)
            .Should()
            .HaveNameEndingWith(RequestsSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void RequestsAndResponses_Should_BeSealed()
    {
        var result = Types.InAssembly(ContractsAssembly)
            .That()
            .ResideInNamespaceEndingWith(ResponsesSuffix)
            .Or()
            .ResideInNamespaceEndingWith(RequestsSuffix)
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
