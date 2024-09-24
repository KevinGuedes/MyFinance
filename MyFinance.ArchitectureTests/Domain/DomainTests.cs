using MyFinance.ArchitectureTests.Common;
using MyFinance.Domain.Common;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MyFinance.ArchitectureTests.Domain;

public sealed class DomainTests : BaseArchitectureTest
{
    private const string EntitiesNamespace = "MyFinance.Domain.Entities";
    private const string DomainAbstractionsNamespace = "MyFinance.Domain.Abstractions";

    [Fact]
    public void DomainEntities_Should_BeSealed()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEntities_Should_ResideInEntitiesNamespace()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .Should()
            .ResideInNamespaceContaining(EntitiesNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEntities_Should_InheritFromBaseEntity()
    {
        var result = Types.InNamespace(EntitiesNamespace)
            .Should()
            .Inherit(typeof(Entity))
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEntities_Should_HavePrivateParameterlessConstructor()
    {
        var entities = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = entities
            .Select(entity => entity.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic))
            .Where(constructors => constructors.All(constructor => constructor.GetParameters().Length != 0));

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void DomainInterfaces_Should_HaveInterfacesPrefix()
    {
        var startsWithInterfacePrefix = Types.InNamespace(DomainAbstractionsNamespace)
            .Should()
            .HaveNameStartingWith(InterfacesPrefix)
            .GetResult();

        var areInterfaces = Types.InNamespace(DomainAbstractionsNamespace)
            .Should()
            .BeInterfaces()
            .GetResult();

        startsWithInterfacePrefix.IsSuccessful.Should().BeTrue();
        areInterfaces.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEntities_Should_HavePrivateSetters()
    {
        var entities = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var entity in entities)
        {
            var setMethods = entity
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(property => property.GetSetMethod(true))
                .Where(setMethod => setMethod is not null && 
                    !setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(typeof(IsExternalInit)));

            if(setMethods.Any(setMethod => !setMethod!.IsPrivate))
                failingTypes.Add(entity);
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void BaseEntity_Should_HavePrivateSetters()
    {
        var setMethods = typeof(Entity)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => property.GetSetMethod(true))
            .Where(setMethod => setMethod is not null &&
                 !setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(typeof(IsExternalInit)));
        
        setMethods.Should().OnlyContain(setMethod => setMethod!.IsPrivate);
    }
}