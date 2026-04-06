namespace Qaeq.Exceptions;

/// <summary>
/// Thrown when SQL execution fails. Wraps the underlying SQL error with a
/// categorized <see cref="Category"/> for structured error handling.
/// </summary>
public class ExecutionException : QaeqException
{
    /// <summary>
    /// The category of execution failure.
    /// </summary>
    public ExecutionErrorCategory Category { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ExecutionException"/> with a message and category.
    /// </summary>
    /// <param name="message">A description of the execution failure.</param>
    /// <param name="category">The categorized type of failure.</param>
    public ExecutionException(string message, ExecutionErrorCategory category)
        : base(message)
    {
        Category = category;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ExecutionException"/> with a message, category, and inner exception.
    /// </summary>
    /// <param name="message">A description of the execution failure.</param>
    /// <param name="category">The categorized type of failure.</param>
    /// <param name="innerException">The underlying SQL exception.</param>
    public ExecutionException(string message, ExecutionErrorCategory category, Exception innerException)
        : base(message, innerException)
    {
        Category = category;
    }
}
