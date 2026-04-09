using Qaeq.Internal;
using System.Linq.Expressions;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Internal implementation of <see cref="INavigationBuilder{T, TNav}"/>
/// </summary>
internal class NavigationBuilder<T, TNav> : INavigationBuilder<T, TNav>
    where T : class
    where TNav : class
{
    private readonly EntityBuilder<T> _parent;
    private readonly RelationshipType _cardinality;

    public NavigationBuilder(EntityBuilder<T> parent, RelationshipType cardinality)
    {
        _parent = parent;
        _cardinality = cardinality;
    }

    public void WithForeignKey<TKey>(Expression<Func<T, TKey>> foreignKeySelector)
    {
        var propertyName = ExpressionHelper.GetPropertyNames(foreignKeySelector).Single();
        _parent.RegisterRelationship(
            typeof(TNav),
            _cardinality,
            propertyName,
            foreignKeyOnSource: true);
    }

    public void WithForeignKeyOn<TKey>(Expression<Func<TNav, TKey>> foreignKeySelector)
    {
        var propertyName = ExpressionHelper.GetPropertyNames(foreignKeySelector).Single();
        _parent.RegisterRelationship(
            typeof(TNav),
            _cardinality,
            propertyName,
            foreignKeyOnSource: false);
    }
}
