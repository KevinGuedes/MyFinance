using Microsoft.AspNetCore.Mvc;
using MyFinance.ArchitectureTests.Common;
using MyFinance.Presentation.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFinance.ArchitectureTests.Presentation;

public sealed class PresentationTests : BaseArchitectureTest
{
    private const string ControllersSuffix = "Controller";
    private const string ControllersNamespace = "MyFinance.Presentation.Controllers";

    [Fact]
    public void Controllers_Should_HaveControllersSuffix()
    {
        var result = Types.InAssembly(PresentationAssembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .And()
            .AreClasses()
            .Should()
            .HaveNameEndingWith(ControllersSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_ResideInCorrectNamespace()
    {
        var result = Types.InAssembly(PresentationAssembly)
            .That()
            .Inherit(typeof(ControllerBase))
            .And()
            .AreClasses()
            .Should()
            .ResideInNamespace(ControllersNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Controllers_Should_HaveASwaggerTag()
    {
        var result = Types.InAssembly(PresentationAssembly)
            .That()
            .Inherit(typeof(ApiController))
            .Should()
            .HaveCustomAttribute(typeof(SwaggerTagAttribute))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApiController_Should_HaveCorrectAttributes()
    {
        var hasRouteAttribute = Types.InNamespace(ControllersNamespace)
            .That()
            .HaveName(nameof(ControllerBase))
            .Should()
            .HaveCustomAttribute(typeof(RouteAttribute))
            .GetResult();

        var hasProducesAttribute = Types.InNamespace(ControllersNamespace)
            .That()
            .HaveName(nameof(ControllerBase))
            .Should()
            .HaveCustomAttribute(typeof(ProducesAttribute))
            .GetResult();

        var hasControllerAttribute = Types.InNamespace(ControllersNamespace)
            .That()
            .HaveName(nameof(ControllerBase))
            .Should()
            .HaveCustomAttribute(typeof(ApiControllerAttribute))
            .GetResult();

        var hasSwaggerResponseAttribute = Types.InNamespace(ControllersNamespace)
           .That()
           .HaveName(nameof(ControllerBase))
           .Should()
           .HaveCustomAttribute(typeof(SwaggerResponseAttribute))
           .GetResult();

        hasRouteAttribute.IsSuccessful.Should().BeTrue();
        hasProducesAttribute.IsSuccessful.Should().BeTrue();
        hasControllerAttribute.IsSuccessful.Should().BeTrue();
        hasSwaggerResponseAttribute.IsSuccessful.Should().BeTrue();
    }
}
