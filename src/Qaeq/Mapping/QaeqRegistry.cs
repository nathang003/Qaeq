using Qaeq.Exceptions;

namespace Qaeq.Mapping;

/// <summary>
/// Static entry point for configuring entity metadata.
/// Call <see cref="Configure"/> once at application startup.
/// The registry is frozen after configuration and shared across all context instances.
/// </summary>
public static class QaeqRegistry
{
    /// <summary>
    /// Indicates whether the registry has been configured.
    /// </summary>
    public static bool IsConfigured => throw new NotImplementedException();

    /// <summary>
    /// Configures the metadata registry with entity mappings and relationships.
    /// May only be called once. Subsequent calls throw <see cref="ConfigurationException"/>.
    /// </summary>
    /// <param name="configure">An action that configures entities via <see cref="IMetadataRegistry"/>.</param>
    /// <exception cref="ConfigurationException">Thrown if called more than once.</exception>
    public static void Configure(Action<IMetadataRegistry> configure)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves the computed model descriptor for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <returns>The model descriptor for <typeparamref name="T"/>.</returns>
    /// <exception cref="ConfigurationException">Thrown if the registry has not been configured.</exception>
    /// <exception cref="MappingException">Thrown if <typeparamref name="T"/> has not been registered.</exception>
    public static IModelDescriptor GetDescriptor<T>() where T : class
    {
        throw new NotImplementedException();
    }
}
