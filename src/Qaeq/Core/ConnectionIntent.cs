namespace Qaeq.Core;

/// <summary>
/// Specifies the application intent for a database connection,
/// enabling routing to read replicas when appropriate.
/// </summary>
public enum ConnectionIntent
{
    /// <summary>Use the default read-write connection.</summary>
    Default,

    /// <summary>Use a read-only connection, enabling read replica routing.</summary>
    ReadOnly
}
