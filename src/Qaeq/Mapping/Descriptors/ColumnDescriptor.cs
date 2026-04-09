namespace Qaeq.Mapping.Descriptors;

/// <summary>
/// Describes a mapped column on an entity.
/// </summary>
public sealed record ColumnDescriptor
{
    /// <summary>
    /// The CLR property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// The database column name.
    /// </summary>
    public required string ColumnName { get; init; }

    /// <summary>
    /// The CLR property type.
    /// </summary>
    public required Type ClrType { get; init; }

    /// <summary>
    /// The SQL type name if explicitly specified via <see cref="Attributes.ColumnAttribute.TypeName"/>
    /// otherwise <see langword="null"/>
    /// </summary>
    public string? SqlTypeName { get; init; }

    /// <summary>
    /// The ordinal position if explicitly specified via <see cref="Attributes.ColumnAttribute.Order"/>
    /// </summary>
    public int Order { get; init; } = -1;

    /// <summary>
    /// Whether the property type is nullable (derived from the CLR type).
    /// </summary>
    public bool IsNullable { get; init; }
}
