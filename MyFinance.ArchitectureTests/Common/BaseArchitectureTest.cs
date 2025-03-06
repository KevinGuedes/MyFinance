using System.Reflection;

namespace MyFinance.ArchitectureTests.Common;

public abstract class BaseArchitectureTest
{
    protected static readonly string InterfacesPrefix = "I";
    protected static readonly string DomainAssemblyName = "MyFinance.Domain";
    protected static readonly string ContractsAssemblyName = "MyFinance.Contracts";
    protected static readonly string ApplicationAssemblyName = "MyFinance.Application";
    protected static readonly string InfrastructureAssemblyName = "MyFinance.Infrastructure";
    protected static readonly string PresentationAssemblyName = "MyFinance.Presentation";
}
