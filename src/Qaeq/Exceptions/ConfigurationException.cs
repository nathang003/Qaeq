namespace Qaeq.Exceptions;

/// <summary>
/// Thrown when the Qaeq library is configured incorrectly.
/// Examples: missing connection string, accessing change tracker when tracking is disabled,
/// modifying the metadata registry after it has been frozen.
/// </summary>
public class ConfigurationException : QaeqException
{
    /// <summary>
    /// Initializes a new instance of <see cref="ConfigurationException"/> with a message.
    /// </summary>
    /// <param name="message">A description of the configuration error.</param>
    public ConfigurationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="ConfigurationException"/> with a message and inner exception.
    /// </summary>
    /// <param name="message">A description of the configuration error.</param>
    /// <param name="innerException">The underlying exception that caused this error.</param>
    public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
}
