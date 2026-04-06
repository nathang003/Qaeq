using Qaeq.Mapping;
using System.Data.Common;

namespace Qaeq.Execution;

/// <summary>
/// Converts database result sets into typed objects or dynamic representations.
/// </summary>
public interface IMaterializer
{
    /// <summary>
    /// Reads the current row from the reader and materializes it into an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The entity type to materialize.</typeparam>
    /// <param name="reader">An open data reader positioned on a row.</param>
    /// <param name="descriptor">The model descriptor for <typeparamref name="T"/>.</param>
    /// <returns>A populated instance of <typeparamref name="T"/>.</returns>
    /// <exception cref="Exceptions.IntegrityException">Thrown if a column value cannot be assigned to the target property type.</exception>
    T Materialize<T>(DbDataReader reader, IModelDescriptor descriptor) where T : class;

    /// <summary>
    /// Reads all remaining rows from the reader and streams them as instances of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The entity type to materialize.</typeparam>
    /// <param name="reader">An open data reader.</param>
    /// <param name="descriptor">The model descriptor for <typeparamref name="T"/>.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An async enumerable of materialized entities.</returns>
    IAsyncEnumerable<T> MaterializeStream<T>(DbDataReader reader, IModelDescriptor descriptor, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Reads the current row from the reader and materializes it as a dynamic object.
    /// Every column in the result set becomes a property.
    /// </summary>
    /// <param name="reader">An open data reader positioned on a row.</param>
    /// <returns>A dynamic object with properties matching the result columns.</returns>
    dynamic MaterializeDynamic(DbDataReader reader);
}
