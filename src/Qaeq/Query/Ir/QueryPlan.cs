namespace Qaeq.Query.Ir;

/// <summary>
/// Represents the accumulated state of a query built through <see cref="IQueryBuilder{T}"/>.
/// Contains the intermediate representation nodes that the compiler translates into SQL.
/// </summary>
/// <typeparam name="T">The root entity type being queried.</typeparam>
/// <remarks>
/// The specific IR node types (SelectNode, WhereNode, JoinNode, etc.) will be
/// defined in Phase 3. This class serves as the container that carries them
/// from the query builder to the compiler.
/// </remarks>
public class QueryPlan<T> where T : class
{
}
