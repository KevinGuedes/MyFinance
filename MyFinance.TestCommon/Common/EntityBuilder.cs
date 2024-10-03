using MyFinance.Domain.Common;
using System.Reflection;

namespace MyFinance.TestCommon.Common;

public abstract class EntityBuilder<TEntity>() where TEntity : Entity
{
    public abstract TEntity Build();

    protected static TEntity BuildEntity(object values)
    {
        var (instance, instanceProps) = GetEntityInstanceInfo();

        var receivedObjectProperties = values.GetType().GetProperties();

        foreach (var objectProp in receivedObjectProperties)
        {
            var value = objectProp.GetValue(values);

            if (value is null)
                continue;

            var instanceProp
                = instanceProps.FirstOrDefault(instanceProp => instanceProp.Name == objectProp.Name);

            if (instanceProp is null)
                continue;

            var declaringType = instanceProp.DeclaringType;

            if (declaringType == typeof(TEntity))
            {
                instanceProp.SetValue(instance, value);
            }
            else
            {
                var originalPropInfo = declaringType!.GetProperty(instanceProp.Name);

                if (originalPropInfo is null)
                    continue;

                originalPropInfo.SetValue(instance, value);
            }
        }

        return instance;
    }

    private static (TEntity, PropertyInfo[]) GetEntityInstanceInfo()
    {
        var instance = Activator.CreateInstance(typeof(TEntity), true) ??
            throw new InvalidOperationException("Could not create an instance of the entity.");

        var instanceProps = instance.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var entityProps = typeof(Entity).GetProperties();
        var createdOnUtcProp = entityProps.FirstOrDefault(prop => prop.Name == nameof(Entity.CreatedOnUtc));
        createdOnUtcProp!.SetValue(instance, null);

        return ((instance as TEntity)!, instanceProps);
    }
}