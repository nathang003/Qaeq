using Qaeq.Query;
using Qaeq.Tracking;

namespace Qaeq.Core;

/// <summary>
/// Default implementation of <see cref="IDbContext"/>.
/// Create one instance per unit of work. Dispose when done.
/// </summary>
public class QaeqContext : IDbContext
{
    private readonly QaeqOptions _options;

    /// <inheritdoc />
    public IChangeTracker ChangeTracker => throw new NotImplementedException();

    /// <summary>
    /// Initializes a new context with the specified options.
    /// </summary>
    /// <param name="options">The configuration options for this context.</param>
    public QaeqContext(QaeqOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Creates a new context using a builder pattern for configuration.
    /// </summary>
    /// <param name="configure">An action to configure the options.</param>
    /// <returns>A configured <see cref="QaeqContext"/> instance.</returns>
    public static QaeqContext Configure(Action<QaeqOptions> configure)
    {
        var options = new QaeqOptions();
        configure(options);
        return new QaeqContext(options);
    }

    /// <inheritdoc />
    public IQueryBuilder<T> Query<T>() where T : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<List<T>> RawQueryAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default) where T : class
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<List<dynamic>> RawQueryDynamicAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Will be implemented when connection management is built in Phase 4.
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        // Will be implemented when connection management is built in Phase 4.
        return ValueTask.CompletedTask;
    }
}
