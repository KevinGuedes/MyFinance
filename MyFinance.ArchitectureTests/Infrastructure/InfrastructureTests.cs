using MyFinance.ArchitectureTests.Common;

namespace MyFinance.ArchitectureTests.Infrastructure;

public class InfrastructureTests : BaseArchitectureTest
{
   private const string InfrastructureServicesNamespace = "MyFinance.Infrastructure.Services";
   private const string InfrastructureAbstractionsNamespace = "MyFinance.Infrastructure.Abstractions";

    [Fact]
    public void Services_Should_BeSealed()
    {
        var result = Types.InNamespace(InfrastructureServicesNamespace)
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void InfrastructureInterfaces_Should_HaveInterfacesPrefix()
    {
        var startsWithInterfacePrefix = Types.InNamespace(InfrastructureAbstractionsNamespace)
            .Should()
            .HaveNameStartingWith(InterfacesPrefix)
            .GetResult();

        var areInterfaces = Types.InNamespace(InfrastructureAbstractionsNamespace)
            .Should()
            .BeInterfaces()
            .GetResult();

        startsWithInterfacePrefix.IsSuccessful.Should().BeTrue();
        areInterfaces.IsSuccessful.Should().BeTrue();
    }
}
