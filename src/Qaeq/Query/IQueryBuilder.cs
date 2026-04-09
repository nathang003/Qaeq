using Qaeq.Core;
using System.Linq.Expressions;

namespace Qaeq.Query;

/// <summary>
/// Fluent query builder for constructing and executing queries against an entity type.
/// Methods are chained to build the query; terminal methods execute it.
/// </summary>
/// <typeparam name="T">The root entity type being queried.</typeparam>
public interface IQueryBuilder<T> where T : class
{
    /// <summary>
    /// Filters the query results by the specified predicate.
    /// Multiple calls are combined with AND.
    /// </summary>
    /// <param name="predicate">A boolean expression to filter results.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> Where(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Orders the results by the specified property in ascending order.
    /// </summary>
    /// <typeparam name="TKey">The type of the property to order by.</typeparam>
    /// <param name="keySelector">An expression selecting the sort property.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Orders the results by the specified property in descending order.
    /// </summary>
    /// <typeparam name="TKey">The type of the property to order by.</typeparam>
    /// <param name="keySelector">An expression selecting the sort property.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Adds a secondary ascending sort to the query.
    /// Must be called after <see cref="OrderBy{TKey}"/> or <see cref="OrderByDescending{TKey}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the property to order by.</typeparam>
    /// <param name="keySelector">An expression selecting the sort property.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Adds a secondary descending sort to the query.
    /// Must be called after <see cref="OrderBy{TKey}"/> or <see cref="OrderByDescending{TKey}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the property to order by.</typeparam>
    /// <param name="keySelector">An expression selecting the sort property.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Skips the specified number of results. Used for pagination.
    /// When combined with <see cref="Take"/>, the default record cap is suppressed.
    /// </summary>
    /// <param name="count">The number of results to skip.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> Skip(int count);

    /// <summary>
    /// Limits the results to the specified number. Used for pagination.
    /// When combined with <see cref="Skip"/>, the default record cap is suppressed.
    /// </summary>
    /// <param name="count">The maximum number of results to return.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> Take(int count);

    /// <summary>
    /// Forces a JOIN-based loading strategy for the specified navigation property,
    /// overriding the default batch loading behavior for one-to-many relationships.
    /// </summary>
    /// <typeparam name="TNav">The navigation entity type.</typeparam>
    /// <param name="navigation">An expression selecting the navigation property to include via JOIN.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> Include<TNav>(Expression<Func<T, TNav>> navigation);

    /// <summary>
    /// Removes the default record cap, allowing the query to return all matching results.
    /// </summary>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> NoCap();

    /// <summary>
    /// Sets the application intent for the connection used by this query.
    /// Use <see cref="ConnectionIntent.ReadOnly"/> to route to read replicas.
    /// </summary>
    /// <param name="intent">The connection intent.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> WithIntent(ConnectionIntent intent);

    /// <summary>
    /// Overrides the default query timeout for this query.
    /// </summary>
    /// <param name="timeout">The timeout duration.</param>
    /// <returns>This builder for continued chaining.</returns>
    IQueryBuilder<T> WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Executes the query and returns all results as a list.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of materialized entities.</returns>
    Task<List<T>> ToListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns the first result, or null if no results are found.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The first entity, or null.</returns>
    Task<T?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns exactly one result.
    /// Throws if zero or more than one result is found.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The single matching entity.</returns>
    Task<T> SingleAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and streams results asynchronously as they are read from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An async enumerable of materialized entities.</returns>
    IAsyncEnumerable<T> ToAsyncEnumerable(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the query and returns all results as dynamic objects.
    /// Each column in the result set becomes a property on the dynamic object.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of dynamic objects.</returns>
    Task<List<dynamic>> ToDynamicListAsync(CancellationToken cancellationToken = default);
}