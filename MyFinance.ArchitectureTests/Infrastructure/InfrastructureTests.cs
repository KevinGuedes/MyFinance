using MyFinance.ArchitectureTests.Common;
using MyFinance.Infrastructure.Abstractions;
using System.Reflection;

namespace MyFinance.ArchitectureTests.Infrastructure;

public sealed class InfrastructureTests : BaseArchitectureTest
{
    private const string InfrastructureServicesNamespace = "MyFinance.Infrastructure.Services";
    private const string InfrastructureAbstractionsNamespace = "MyFinance.Infrastructure.Abstractions";
    private const string OptionsSuffix = "Options";
    private readonly Assembly _infrastructureAssembly = Assembly.Load(InfrastructureAssemblyName);

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
    public void ServiceOptions_Should_ResideInServicesNamespace()
    {
        var result = Types.InAssembly(_infrastructureAssembly)
            .That()
            .HaveNameEndingWith(OptionsSuffix)
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespace(InfrastructureServicesNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ServiceOptions_Should_BeSealed()
    {
        var result = Types.InAssembly(_infrastructureAssembly)
            .That()
            .HaveNameEndingWith(OptionsSuffix)
            .And()
            .AreClasses()
            .Should()
            .BeSealed()
            .And()
            .ImplementInterface(typeof(IValidatableOptions))
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
