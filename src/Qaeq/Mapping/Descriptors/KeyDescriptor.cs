namespace Qaeq.Mapping.Descriptors;

/// <summary>
/// Describes the primary key of an entity. Supports composite keys.
/// </summary>
public sealed record KeyDescriptor
{
    /// <summary>
    /// The CLR property names that compose the key.
    /// </summary>
    public required IReadOnlyList<string> PropertyNames { get; init; }

    /// <summary>
    /// The corresponding database column names.
    /// </summary>
    public required IReadOnlyList<string> ColumnNames { get; init; }
}
