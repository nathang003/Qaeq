namespace Qaeq.Exceptions;

/// <summary>
/// Categorizes the type of failure that occurred during SQL execution.
/// </summary>
public enum ExecutionErrorCategory
{
    /// <summary>The query exceeded the configured timeout.</summary>
    Timeout,

    /// <summary>The database connection could not be established or was lost.</summary>
    Connection,

    /// <summary>A constraint violation occurred (unique, foreign key, check, etc.).</summary>
    Constraint,

    /// <summary>The query was chosen as a deadlock victim.</summary>
    Deadlock,

    /// <summary>An unrecognized SQL error occurred. Inspect the inner exception for details.</summary>
    Unknown
}

