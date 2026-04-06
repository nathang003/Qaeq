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
    /// Initializes a new instance of <see cref="TableAttribute"/> with the specified table name.
    /// </summary>
    /// <param name="name">The database table name.</param>
    public TableAttribute(string name)
    {
        Name = name;
    }
}
