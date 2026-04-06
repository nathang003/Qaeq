namespace Qaeq.Tracking;

/// <summary>
/// Represents the result of comparing tracked entity snapshots to their current state.
/// </summary>
public class ChangeSet
{
    /// <summary>
    /// Entities that have been added to tracking since the last snapshot.
    /// </summary>
    public IReadOnlyList<object> Added { get; init; } = [];

    /// <summary>
    /// Entities that have been modified since their snapshot was captured.
    /// Per-property change details will be available via <c>ModifiedEntry</c> (defined in Phase 6).
    /// </summary>
    public IReadOnlyList<object> Modified { get; init; } = [];

    /// <summary>
    /// Entities that have been removed from tracking since the last snapshot.
    /// </summary>
    public IReadOnlyList<object> Removed { get; init; } = [];
}
