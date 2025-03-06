using MyFinance.ArchitectureTests.Common;
using System.Reflection;

namespace MyFinance.ArchitectureTests.Layer;

public sealed class LayerTests : BaseArchitectureTest
{
    private readonly Assembly _domainAssembly = Assembly.Load(DomainAssemblyName);
    private readonly Assembly _infrastructureAssembly = Assembly.Load(InfrastructureAssemblyName);
    private readonly Assembly _applicationAssembly = Assembly.Load(ApplicationAssemblyName);
    private readonly Assembly _contractsAssembly = Assembly.Load(ContractsAssemblyName);
    private readonly Assembly _presentationAssembly = Assembly.Load(PresentationAssemblyName);

    [Fact]
    public void DomainLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(_domainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                ApplicationAssemblyName,
                ContractsAssemblyName,
                PresentationAssemblyName,
                InfrastructureAssemblyName)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                PresentationAssemblyName,
                InfrastructureAssemblyName)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void PresentationLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(_presentationAssembly)
          .ShouldNot()
          .HaveDependencyOnAny(DomainAssemblyName)
          .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ContractsLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(_contractsAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                ApplicationAssemblyName,
                PresentationAssemblyName,
                InfrastructureAssemblyName)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(_infrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(PresentationAssemblyName)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
