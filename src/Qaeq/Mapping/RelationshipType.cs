namespace Qaeq.Mapping;

/// <summary>
/// Describes the cardinality of a relationship between two entities.
/// </summary>
public enum RelationshipType
{
    /// <summary>A one-to-one relationship. Loaded via JOIN by default.</summary>
    OneToOne,

    /// <summary>A many-to-one relationship. Loaded via JOIN by default.</summary>
    ManyToOne,

    /// <summary>A one-to-many relationship. Loaded via parallel batch by default.</summary>
    OneToMany
}
