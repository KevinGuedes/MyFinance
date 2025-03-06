using FluentValidation;
using MyFinance.Application.Abstractions.RequestHandling.Commands;
using MyFinance.Application.Abstractions.RequestHandling.Queries;
using MyFinance.Application.Common.Errors;
using MyFinance.ArchitectureTests.Common;
using System.Reflection;

namespace MyFinance.ArchitectureTests.Application;

public sealed class ApplicationTests : BaseArchitectureTest
{
    private const string HandlersSuffix = "Handler";
    private const string QueriesSuffix = "Query";
    private const string CommandsSuffix = "Command";
    private const string ErrorsSuffix = "Error";
    private const string ValidatorsSuffix = "Validator";
    private const string ApplicationErrorsNamespace = "MyFinance.Application.Common.Errors";
    private const string ApplicationUseCasesNamespace = "MyFinance.Application.UseCases";
    private const string ApplicationCustomValidatorsNamespace = "MyFinance.Application.Common.CustomValidators";
    private const string ApplicationAbstractionsNamespace = "MyFinance.Application.Abstractions";
    private readonly Assembly _applicationAssembly = Assembly.Load(ApplicationAssemblyName);

    [Fact]
    public void CommandHandlers_Should_HaveHandlersSuffix()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .HaveNameEndingWith(HandlersSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlers_Should_BeSealed()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_HaveHandlersSuffix()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith(HandlersSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandlers_Should_BeSealed()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_HaveCommandsSuffix()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith(CommandsSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_BeSealed()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Or()
            .ImplementInterface(typeof(ICommand))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_HaveQueriesSuffix()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith(QueriesSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Queries_Should_BeSealed()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void RequestValidators_Should_BeSealed()
    {
        var result = Types.InNamespace(ApplicationUseCasesNamespace)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void RequestValidators_Should_HaveValidatorSuffix()
    {
        var result = Types.InNamespace(ApplicationUseCasesNamespace)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith(ValidatorsSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void ApplicationInterfaces_Should_HaveInterfacePrefix()
    {
        var types = Types.InNamespace(ApplicationAbstractionsNamespace);

        var startsWithInterfacePrefix = types
            .Should()
            .HaveNameStartingWith(InterfacesPrefix)
            .GetResult();

        var areInterfaces = types
            .Should()
            .BeInterfaces()
            .GetResult();

        startsWithInterfacePrefix.IsSuccessful.Should().BeTrue();
        areInterfaces.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Errors_Should_BeSealed()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .ResideInNamespace(ApplicationErrorsNamespace)
            .And()
            .Inherit(typeof(BaseError))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Errors_Should_HaveErrorsSuffix()
    {
        var result = Types.InAssembly(_applicationAssembly)
            .That()
            .Inherit(typeof(BaseError))
            .Should()
            .HaveNameEndingWith(ErrorsSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CustomValidators_Should_BeStatic()
    {
        var result = Types.InNamespace(ApplicationCustomValidatorsNamespace)
            .Should()
            .BeStatic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CustomValidators_Should_HaveValidatorsSuffix()
    {
        var result = Types.InNamespace(ApplicationCustomValidatorsNamespace)
            .Should()
            .HaveNameEndingWith(ValidatorsSuffix)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
