using System.Reflection;

namespace MyFinance.ArchitectureTests.Common;

public abstract class BaseArchitectureTest
{
    protected static readonly string InterfacesPrefix = "I";

    protected static readonly string DomainAssemblyName = "MyFinance.Domain";
    protected static readonly Assembly DomainAssembly = Assembly.Load(DomainAssemblyName);

    protected static readonly string ContractsAssemblyName = "MyFinance.Contracts";
    protected static readonly Assembly ContractsAssembly = Assembly.Load(ContractsAssemblyName);

    protected static readonly string ApplicationAssemblyName = "MyFinance.Application";
    protected static readonly Assembly ApplicationAssembly = Assembly.Load(ApplicationAssemblyName);

    protected static readonly string InfrastructureAssemblyName = "MyFinance.Infrastructure";
    protected static readonly Assembly InfrastructureAssembly = Assembly.Load(InfrastructureAssemblyName);

    protected static readonly string PresentationAssemblyName = "MyFinance.Presentation";
    protected static readonly Assembly PresentationAssembly = Assembly.Load(PresentationAssemblyName);
}
