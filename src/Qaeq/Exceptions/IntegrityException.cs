namespace Qaeq.Exceptions;

/// <summary>
/// Thrown when a data integrity violation is detected.
/// Examples: CLR type does not match SQL column type, schema drift detected between
/// context creation and change tracking operations.
/// </summary>
public class IntegrityException : QaeqException
{
    /// <summary>
    /// Initializes a new instance of <see cref="IntegrityException"/> with a message.
    /// </summary>
    /// <param name="message">A description of the integrity violation.</param>
    public IntegrityException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="IntegrityException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">A description of the integrity violation.</param>
    /// <param name="innerException">The underlying exception that caused this error.</param>
    public IntegrityException(string message, Exception innerException) : base(message, innerException) { }
}
