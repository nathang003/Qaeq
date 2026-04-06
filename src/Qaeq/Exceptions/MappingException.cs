namespace Qaeq.Exceptions;

/// <summary>
/// Thrown when model-to-database mapping configuration is invalid.
/// Examples: missing table, missing column, misconfigured relationship keys.
/// </summary>
public class MappingException : QaeqException
{
    /// <summary>
    /// Initializes a new instance of <see cref="MappingException"/> with a message.
    /// </summary>
    /// <param name="message">A description of the mapping error.</param>
    public MappingException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="MappingException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">A description of the mapping error.</param>
    /// <param name="innerException">The underlying exception that caused this error.</param>
    public MappingException(string message, Exception innerException) : base(message, innerException) { }
}
