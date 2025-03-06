using MyFinance.ArchitectureTests.Common;
using System.Reflection;

namespace MyFinance.ArchitectureTests.Contracts;

public sealed class ContractsTest : BaseArchitectureTest
{
    private const string RequestsSuffix = "Request";
    private const string ResponsesSuffix = "Response";
    private readonly Assembly _contractsAssembly = Assembly.Load(ContractsAssemblyName);

    [Fact]
    public void Responses_Should_HaveResponsesSuffix()
    {
        var result = Types.InAssembly(_contractsAssembly)
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
        var result = Types.InAssembly(_contractsAssembly)
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
        var result = Types.InAssembly(_contractsAssembly)
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
