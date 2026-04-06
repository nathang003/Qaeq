namespace Qaeq.Exceptions;

/// <summary>
/// Base exception for all errors originating from the Qaeq library.
/// Catch this type to handle any Qaeq error generically.
/// </summary>
public class QaeqException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="QaeqException"/> with a message.
    /// </summary>
    /// <param name="message">A description of the error.</param>
    public QaeqException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="QaeqException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">A description of the error.</param>
    /// <param name="innerException">The underlying exception that caused this error.</param>
    public QaeqException(string message, Exception innerException) : base(message, innerException) { }
}
