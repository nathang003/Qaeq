namespace Qaeq.Exceptions;

/// <summary>
/// Thrown when a query cannot be compiled into valid SQL.
/// Examples: unsupported expression in a Where clause, invalid query structure.
/// </summary>
public class QueryException : QaeqException
{
    /// <summary>
    /// Initializes a new instance of <see cref="QueryException"/> with a message.
    /// </summary>
    /// <param name="message">A description of the query compilation failure.</param>
    public QueryException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="QueryException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">A description of the query compilation failure.</param>
    /// <param name="innerException">The underlying exception that caused this error.</param>
    public QueryException(string message, Exception innerException) : base(message, innerException) { }
}
