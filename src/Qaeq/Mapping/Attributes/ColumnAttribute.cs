namespace Qaeq.Mapping.Attributes;

/// <summary>
/// Maps a property to a database column.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ColumnAttribute : Attribute
{
    /// <summary>
    /// The name of the database column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The ordinal position of the column. A value of -1 indicates unspecified order.
    /// </summary>
    public int Order { get; set; } = -1;

    /// <summary>
    /// An optional SQL type name hint (e.g., "nvarchar(50)", "decimal(18,2)").
    /// Used during schema validation.
    /// </summary>
    public string? TypeName { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ColumnAttribute"/> with the specified column name.
    /// </summary>
    /// <param name="name">The database column name.</param>
    public ColumnAttribute(string name)
    {
        Name = name;
    }
}
