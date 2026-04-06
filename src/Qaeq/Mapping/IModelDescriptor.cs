namespace Qaeq.Mapping;

/// <summary>
/// A read-only, computed description of a mapped entity type.
/// Combines information from <see cref="Attributes.TableAttribute"/>,
/// <see cref="Attributes.ColumnAttribute"/>, and the <see cref="IMetadataRegistry"/>
/// into a single queryable representation.
/// </summary>
/// <remarks>
/// The descriptor record types (<c>ColumnDescriptor</c>, <c>KeyDescriptor</c>,
/// <c>RelationshipDescriptor</c>) will be defined in Phase 1 when the mapping
/// logic is implemented. This interface establishes the contract shape.
/// </remarks>
public interface IModelDescriptor
{
    /// <summary>
    /// The CLR type this descriptor represents.
    /// </summary>
    Type ClrType { get; }

    /// <summary>
    /// The database table name.
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// The database schema name.
    /// </summary>
    string Schema { get; }
}
