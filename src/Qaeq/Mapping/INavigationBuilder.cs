using System.Linq.Expressions;

namespace Qaeq.Mapping;

/// <summary>
/// Fluent builder for configuring the foreign key of a navigation relationship.
/// </summary>
/// <typeparam name="T">The source entity type.</typeparam>
/// <typeparam name="TNav">The target navigation entity type.</typeparam>
public interface INavigationBuilder<T, TNav>
    where T : class
    where TNav : class
{
    /// <summary>
    /// Specifies the foreign key on the source entity that defines this relationship.
    /// Typically used with <c>HasOne</c> where the FK lives on the dependent (source) side.
    /// </summary>
    /// <typeparam name="TKey">The type of the foreign key property.</typeparam>
    /// <param name="foreignKeySelector">An expression selecting the foreign key property on <typeparamref name="T"/>.</param>
    void WithForeignKey<TKey>(Expression<Func<T, TKey>> foreignKeySelector);

    /// <summary>
    /// Specifies the foreign key on the target navigation entity that defines this relationship.
    /// Typically used with <c>HasMany</c> where the FK lives on the child (target) side.
    /// </summary>
    /// <typeparam name="TKey">The type of the foreign key property.</typeparam>
    /// <param name="foreignKeySelector">An expression selecting the foreign key property on <typeparamref name="TNav"/>.</param>
    void WithForeignKeyOn<TKey>(Expression<Func<TNav, TKey>> foreignKeySelector);
}
