using System.Linq.Expressions;

namespace Qaeq.Mapping;

/// <summary>
/// Fluent builder for configuring the mapping and relationships of an entity type.
/// </summary>
/// <typeparam name="T">The entity type being configured.</typeparam>
public interface IEntityBuilder<T> where T : class
{
    /// <summary>
    /// Defines the primary key for this entity.
    /// </summary>
    /// <typeparam name="TKey">The type of the key property.</typeparam>
    /// <param name="keySelector">An expression selecting the key property.</param>
    /// <returns>This builder for continued configuration.</returns>
    IEntityBuilder<T> HasKey<TKey>(Expression<Func<T, TKey>> keySelector);

    /// <summary>
    /// Defines a one-to-one or many-to-one navigation to a related entity.
    /// Loaded via JOIN by default.
    /// </summary>
    /// <typeparam name="TNav">The related entity type.</typeparam>
    /// <param name="navigation">An expression selecting the navigation property.</param>
    /// <returns>A navigation builder to configure the foreign key.</returns>
    INavigationBuilder<T, TNav> HasOne<TNav>(Expression<Func<T, TNav?>> navigation) where TNav : class;

    /// <summary>
    /// Defines a one-to-many navigation to a collection of related entities.
    /// Loaded via parallel batch query by default.
    /// </summary>
    /// <typeparam name="TNav">The related entity type in the collection.</typeparam>
    /// <param name="navigation">An expression selecting the collection navigation property.</param>
    /// <returns>A navigation builder to configure the foreign key.</returns>
    INavigationBuilder<T, TNav> HasMany<TNav>(Expression<Func<T, IEnumerable<TNav>>> navigation) where TNav : class;
}
