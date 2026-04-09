using Qaeq.Exceptions;
using System.Collections.Concurrent;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Internal implementation of <see cref="IMetadataRegistry"/>
/// </summary>
internal class MetadataRegistry : IMetadataRegistry
{
    private readonly ConcurrentDictionary<Type, IEntityBuilderInternal> _entityBuilders = new();
    private bool _isFrozen;

    public IEntityBuilder<T> Entity<T>() where T : class
    {
        EnsureNotFrozen();

        var builder = _entityBuilders.GetOrAdd(typeof(T), _ =>
            new EntityBuilder<T>(this));

        return (IEntityBuilder<T>)builder;
    }

    internal EntityBuilder<T>? TryGetBuilder<T>() where T : class
    {
        if (_entityBuilders.TryGetValue(typeof(T), out var builder))
        {
            return (EntityBuilder<T>)builder;
        }

        return null;
    }

    internal IEntityBuilderInternal? TryGetBuilderByType(Type type)
    {
        if (_entityBuilders.TryGetValue(type, out var builder))
        {
            return builder;
        }

        return null;
    }

    internal IReadOnlyCollection<Type> GetRegisteredTypes()
    {
        return _entityBuilders.Keys.ToList();
    }

    internal bool HasBuilder(Type type)
    {
        return _entityBuilders.ContainsKey(type);
    }

    internal void Freeze()
    {
        if (_isFrozen)
        {
            throw new ConfigurationException("The metadata registry has already been configured.");
        }

        _isFrozen = true;
    }

    private void EnsureNotFrozen()
    {
        if (_isFrozen)
        {
            throw new ConfigurationException(
                "The metadata registry is frozen. No further configuration is allowed after QaeqRegistry.Configure has complete.");
        }
    }
}
