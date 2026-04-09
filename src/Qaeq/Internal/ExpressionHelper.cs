using Qaeq.Exceptions;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Qaeq.Internal;

/// <summary>
/// Utility for extracting property names from lambda expressions.
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    /// Extracts property names from a lambda expression. Supports simple member access
    /// (e.g., <c>o => o.Id</c>), anonymous type creation (e.g., <c>o => new { o.TenantId, o.OrderId }</c>),
    /// and expressions wrapped in a Convert node (common with value types boxed to object).
    /// </summary>
    /// <param name="expression">The lambda expression to analyze.</param>
    /// <returns>A list of property names extracted from the expression.</returns>
    /// <exception cref="MappingException">Thrown if the expression form is not supported.</exception>
    public static IReadOnlyList<string> GetPropertyNames(LambdaExpression expression)
    {
        var body = expression.Body;

        // Unwrap Convert nodes (added by compiler when value type is boxed to object)
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unary)
        {
            body = unary.Operand;
        }

        // Simple member access: o => o.Id
        if (body is MemberExpression member)
        {
            return [member.Member.Name];
        }

        // Composite key: o => new { o.TenantId, o.OrderId }
        if (body is NewExpression anonymous && anonymous.Type.IsAnonymousType())
        {
            var names = new List<string>();
            foreach (var argument in anonymous.Arguments)
            {
                if (argument is MemberExpression memberExpression)
                {
                    names.Add(memberExpression.Member.Name);
                }
                else
                {
                    throw new MappingException(
                        $"Unsupported expression in key definition. Expected property access, " +
                        $"but found expression of type '{argument.NodeType}'.");
                }
            }

            return names;
        }

        throw new MappingException(
            $"Unsupported expression in key definition. Expected property access (e.g., o => o.Id) " +
            $"or composite key (e.g., o => new {{ o.TenantId, o.OrderId }}), " +
            $"but found expression of type '{expression.Body.NodeType}'.");
    }

    /// <summary>
    /// Extracts the property name from an expression selecting a navigation property.
    /// </summary>
    /// <param name="expression">The lambda expression selecting the navigation property.</param>
    /// <returns>The property name.</returns>
    /// <exception cref="MappingException">Thrown if the expression form is not supported.</exception>
    public static string GetNavigationPropertyName(LambdaExpression expression)
    {
        var body = expression.Body;

        // Unwrap Convert nodes
        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unary)
        {
            body = unary.Operand;
        }

        if (body is MemberExpression member)
        {
            return member.Member.Name;
        }

        throw new MappingException(
            $"Unsupported navigation expression. Expected property access (e.g., o => o.Customer), " +
            $"but found expression of type '{expression.Body.NodeType}'.");
    }

    private static bool IsAnonymousType(this Type type)
    {
        return type.Name.StartsWith("<>") // Rosalyn-generated anonymous type
            && type.GetCustomAttribute<CompilerGeneratedAttribute>() != null;
    }
}
