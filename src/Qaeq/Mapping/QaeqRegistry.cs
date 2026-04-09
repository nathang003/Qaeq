using Qaeq.Exceptions;
using Qaeq.Mapping.Internal;

namespace Qaeq.Mapping;

/// <summary>
/// Static entry point for configuring entity metadata.
/// Call <see cref="Configure"/> once at application startup.
/// The registry is frozen after configuration and shared across all context instances.
/// </summary>
public static class QaeqRegistry
{
    private static MetadataRegistry? _registry;
    private static bool _isConfigured;

    /// <summary>
    /// Indicates whether the registry has been configured.
    /// </summary>
    public static bool IsConfigured => _isConfigured;

    /// <summary>
    /// Configures the metadata registry with entity mappings and relationships.
    /// May only be called once. Subsequent calls throw <see cref="ConfigurationException"/>.
    /// </summary>
    /// <param name="configure">An action that configures entities via <see cref="IMetadataRegistry"/>.</param>
    /// <exception cref="ConfigurationException">Thrown if called more than once.</exception>
    public static void Configure(Action<IMetadataRegistry> configure)
    {
        if (_isConfigured)
        {
            throw new ConfigurationException(
                "QaeqRegistry has already been configured. Configure can only be called once.");
        }

        _registry = new MetadataRegistry();
        configure(_registry);
        _registry.Freeze();
        _isConfigured = true;
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
        if (!_isConfigured || _registry == null)
        {
            throw new ConfigurationException(
                "QaeqRegistry has not been configured. Call QaeqRegistry.Configure first.");
        }

        return ModelDescriptorCache.GetDescriptor<T>(_registry!);
    }

    /// <summary>
    /// Resets the registry to an unconfigured state.
    /// This method is intended for testing only and should not be used in production code.
    /// </summary>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public static void Reset()
    {
        _registry = null;
        _isConfigured = false;
        ModelDescriptorCache.Clear();
    }
}
