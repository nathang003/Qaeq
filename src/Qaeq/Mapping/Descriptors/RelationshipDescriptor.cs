namespace Qaeq.Mapping.Descriptors;

/// <summary>
/// Describes a navigation relationship between two entities.
/// </summary>
public sealed record RelationshipDescriptor
{
    /// <summary>
    /// The navigation property name on the source entity.
    /// </summary>
    public required string NavigationPropertyName { get; init; }

    /// <summary>
    /// The source entity type.
    /// </summary>
    public required Type SourceType { get; init; }

    /// <summary>
    /// The target/related entity type.
    /// </summary>
    public required Type TargetType { get; init; }

    /// <summary>
    /// The cardinality of the relationship.
    /// </summary>
    public required RelationshipType Cardinality { get; init; }

    /// <summary>
    /// The foreign key property names (supports composite keys).
    /// </summary>
    public required IReadOnlyList<string> ForeignKeyPropertyNames { get; init; }

    /// <summary>
    /// The corresponding database column names for the foreign key.
    /// </summary>
    public required IReadOnlyList<string> ForeignKeyColumnNames { get; init; }

    /// <summary>
    /// <see langword="true"/> if the foreign key resides on the source entity;
    /// <see langword="false"/> if it resides on the target entity.
    /// </summary>
    public required bool ForeignKeyOnSource { get; init; }

    /// <summary>
    /// <see langword="true"/> if both sides of the relationship are declared in the registry;
    /// <see langword="false"/> if only this side is declared.
    /// </summary>
    public bool HasInverse { get; init; }
}
