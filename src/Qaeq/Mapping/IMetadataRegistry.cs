namespace Qaeq.Mapping;

/// <summary>
/// Registry for configuring entity mappings, keys, and relationships.
/// Passed to the consumer during <see cref="QaeqRegistry.Configure"/> for fluent configuration.
/// </summary>
public interface IMetadataRegistry
{
    /// <summary>
    /// Begins configuration for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type to configure.</typeparam>
    /// <returns>An entity builder for fluent configuration.</returns>
    IEntityBuilder<T> Entity<T>() where T : class;
}
