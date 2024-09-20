using System.Reflection;

namespace MyFinance.ArchitectureTests.Common;

public abstract class BaseArchitectureTest
{
    protected static readonly string InterfacesPrefix = "I";

    protected static readonly string EntitiesNamespace = "MyFinance.Domain.Entities";
    protected static readonly string DomainAbstractionsNamespace = "MyFinance.Domain.Abstractions";
    protected static readonly string DomainAssemblyName = "MyFinance.Domain";
    protected static readonly Assembly DomainAssembly = Assembly.Load(DomainAssemblyName);

    protected static readonly string ContractsAssemblyName = "MyFinance.Contracts";
    protected static readonly string RequestsSuffix = "Request";
    protected static readonly string ResponsesSuffix = "Response";
    protected static readonly Assembly ContractsAssembly = Assembly.Load(ContractsAssemblyName);

    protected static readonly string HandlersSuffix = "Handler";
    protected static readonly string QueriesSuffix = "Query";
    protected static readonly string CommandsSuffix = "Command";
    protected static readonly string ErrorsSuffix = "Error";
    protected static readonly string ValidatorsSuffix = "Validator";
    protected static readonly string ApplicationsErrorsNamespace = "MyFinance.Application.Common.Errors";
    protected static readonly string ApplicationCustomValidatorsNamespace = "MyFinance.Application.Common.CustomValidators";
    protected static readonly string ApplicationUseCasesNamespace = "MyFinance.Application.UseCases";
    protected static readonly string ApplicationAssemblyName = "MyFinance.Application";
    protected static readonly string ApplicationAbstractionsNamespace = "MyFinance.Application.Abstractions";
    protected static readonly Assembly ApplicationAssembly = Assembly.Load(ApplicationAssemblyName);

    protected static readonly string InfrastructureServicesNamespace = "MyFinance.Infrastructure.Services";
    protected static readonly string InfrastructureAbstractionsNamespace = "MyFinance.Infrastructure.Abstractions";
    protected static readonly string InfrastructureAssemblyName = "MyFinance.Infrastructure";
    protected static readonly Assembly InfrastructureAssembly = Assembly.Load(InfrastructureAssemblyName);

    protected static readonly string ControllersSuffix = "Controller";
    protected static readonly string ControllersNamespace = "MyFinance.Presentation.Controllers";
    protected static readonly string PresentationAssemblyName = "MyFinance.Presentation";
    protected static readonly Assembly PresentationAssembly = Assembly.Load(PresentationAssemblyName);
}
