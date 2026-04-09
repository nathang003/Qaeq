namespace Qaeq.Mapping.Attributes;

/// <summary>
/// Maps a class to a database table.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TableAttribute : Attribute
{
    /// <summary>
    /// The name of the database table.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The schema the table belongs to. Defaults to "dbo".
    /// </summary>
    public string Schema { get; set; } = "dbo";

    /// <summary>
    /// When <see langword="true"/> (default), only properties decorated with
    /// <see cref="ColumnAttribute"/> are mapped to database columns.
    /// When <see langword="false"/>, all public instance properties are mapped
    /// as columns (excluding virtual properties and those with <see cref="NotMappedAttribute"/>).
    /// Defaults to <see langword="true"/>.
    /// </summary>
    public bool ExplicitColumns { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of <see cref="TableAttribute"/> with the specified table name.
    /// </summary>
    /// <param name="name">The database table name.</param>
    public TableAttribute(string name)
    {
        Name = name;
    }
}
