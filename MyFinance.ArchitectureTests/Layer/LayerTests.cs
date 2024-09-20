using MyFinance.ArchitectureTests.Common;

namespace MyFinance.ArchitectureTests.Layer;

public class LayerTests : BaseArchitectureTest
{
    [Fact]
    public void DomainLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(DomainAssembly)
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
        var result = Types.InAssembly(ApplicationAssembly)
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
        var result = Types.InAssembly(PresentationAssembly)
          .ShouldNot()
          .HaveDependencyOnAny(DomainAssemblyName)
          .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ContractsLayer_Should_HaveAppropriateDependencies()
    {
        var result = Types.InAssembly(ContractsAssembly)
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
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(PresentationAssemblyName)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
