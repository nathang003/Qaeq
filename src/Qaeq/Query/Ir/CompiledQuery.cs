using Microsoft.Data.SqlClient;

namespace Qaeq.Query.Ir;

/// <summary>
/// The output of the query compiler: a parameterized SQL string ready for execution.
/// </summary>
public class CompiledQuery
{
    /// <summary>
    /// The generated SQL command text.
    /// </summary>
    public string Sql { get; init; } = string.Empty;

    /// <summary>
    /// The parameters to bind to the SQL command.
    /// </summary>
    public IReadOnlyList<SqlParameter> Parameters { get; init; } = [];
}
