using Qaeq.Exceptions;

namespace Qaeq.Tracking;

/// <summary>
/// Tracks entity state changes within the lifetime of a single context instance.
/// </summary>
public interface IChangeTracker
{
    /// <summary>
    /// The current tracking mode.
    /// </summary>
    ChangeTrackingMode Mode { get; }

    /// <summary>
    /// Computes the changes between tracked entity snapshots and their current property values.
    /// </summary>
    /// <returns>A <see cref="ChangeSet"/> describing all detected changes.</returns>
    /// <exception cref="ConfigurationException">Thrown if tracking mode is <see cref="ChangeTrackingMode.None"/>.</exception>
    /// <exception cref="IntegrityException">Thrown if schema drift is detected during the integrity check.</exception>
    ChangeSet GetChanges();

    /// <summary>
    /// Begins tracking the specified entity. A snapshot of its current state is captured.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">The entity instance to track.</param>
    void Track<T>(T entity) where T : class;

    /// <summary>
    /// Stops tracking the specified entity and discards its snapshot.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="entity">The entity instance to stop tracking.</param>
    void Untrack<T>(T entity) where T : class;
}
