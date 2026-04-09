using Qaeq.Core;
using System.Data.Common;

namespace Qaeq.Execution;

/// <summary>
/// Creates database connections with the appropriate configuration.
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Creates a new database connection using the default connection intent.
    /// </summary>
    /// <returns>An unopened <see cref="DbConnection"/>.</returns>
    DbConnection Create();

    /// <summary>
    /// Creates a new database connection with the specified application intent.
    /// When <paramref name="intent"/> is <see cref="ConnectionIntent.ReadOnly"/>,
    /// the connection string is modified to include <c>ApplicationIntent=ReadOnly</c>.
    /// </summary>
    /// <param name="intent">The desired connection intent.</param>
    /// <returns>An unopened <see cref="DbConnection"/>.</returns>
    DbConnection Create(ConnectionIntent intent);
}
