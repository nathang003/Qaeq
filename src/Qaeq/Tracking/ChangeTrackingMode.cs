namespace Qaeq.Tracking;

/// <summary>
/// Controls how and when the change tracker captures entity snapshots.
/// </summary>
public enum ChangeTrackingMode
{
    /// <summary>Snapshot every entity on materialization.</summary>
    Full,

    /// <summary>No snapshots are captured. Change detection is disabled.</summary>
    None,

    /// <summary>Snapshots are captured only during configured time windows.</summary>
    Scheduled
}
