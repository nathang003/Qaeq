using Qaeq.Query.Ir;

namespace Qaeq.Query;

/// <summary>
/// Translates a <see cref="QueryPlan{T}"/> into a <see cref="CompiledQuery"/>
/// containing parameterized SQL.
/// </summary>
public interface IQueryCompiler
{
    /// <summary>
    /// Compiles the given query plan into executable SQL.
    /// </summary>
    /// <typeparam name="T">The root entity type.</typeparam>
    /// <param name="plan">The query plan to compile.</param>
    /// <returns>A compiled query with SQL text and parameters.</returns>
    /// <exception cref="Exceptions.QueryException">Thrown if the plan contains expressions that cannot be translated to SQL.</exception>
    CompiledQuery Compile<T>(QueryPlan<T> plan) where T : class;
}
