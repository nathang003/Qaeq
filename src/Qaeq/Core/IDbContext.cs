using Qaeq.Query;
using Qaeq.Tracking;

namespace Qaeq.Core;

/// <summary>
/// The primary entry point for querying and tracking entities.
/// Represents a short-lived unit of work. Create one per operation, use it, and dispose it.
/// </summary>
public interface IDbContext : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// The change tracker for this context instance.
    /// </summary>
    IChangeTracker ChangeTracker { get; }

    /// <summary>
    /// Begins building a query for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type to query.</typeparam>
    /// <returns>A fluent query builder.</returns>
    IQueryBuilder<T> Query<T>() where T : class;

    /// <summary>
    /// Executes a raw SQL query and materializes results into the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type to materialize.</typeparam>
    /// <param name="sql">The SQL query text.</param>
    /// <param name="parameters">An optional object whose properties are used as query parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of materialized entities.</returns>
    Task<List<T>> RawQueryAsync<T>(string sql, object? parameters = null, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Executes a raw SQL query and returns results as dynamic objects.
    /// Each column in the result set becomes a property on the dynamic object.
    /// </summary>
    /// <param name="sql">The SQL query text.</param>
    /// <param name="parameters">An optional object whose properties are used as query parameters.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of dynamic objects.</returns>
    Task<List<dynamic>> RawQueryDynamicAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default);
}
