using Qaeq.Exceptions;
using Qaeq.Internal;
using Qaeq.Mapping.Descriptors;
using System.Linq.Expressions;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Internal implementation of <see cref="IEntityBuilder{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
internal class EntityBuilder<T> : IEntityBuilder<T>, IEntityBuilderInternal where T : class
{
    private readonly MetadataRegistry _registry;
    private KeyDescriptor? _key;
    private readonly List<RelationshipInfo> _relationships = [];

    public Type EntityType => typeof(T);

    internal EntityBuilder(MetadataRegistry registry)
    {
        _registry = registry;
    }

    public IEntityBuilder<T> HasKey<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        var propertyNames = ExpressionHelper.GetPropertyNames(keySelector);
        var columnNames = propertyNames; // For now, assume column name matches property name. Phase 2 will handle [Column] overrides.
        _key = new KeyDescriptor
        {
            PropertyNames = propertyNames,
            ColumnNames = columnNames
        };
        return this;
    }

    public INavigationBuilder<T, TNav> HasOne<TNav>(Expression<Func<T, TNav?>> navigation) where TNav : class
    {
        var navigationPropertyName = ExpressionHelper.GetNavigationPropertyName(navigation);
        _relationships.Add(new RelationshipInfo
        {
            NavigationPropertyName = navigationPropertyName,
            TargetType = typeof(TNav),
            Cardinality = RelationshipType.ManyToOne
        });

        return new NavigationBuilder<T, TNav>(this, RelationshipType.ManyToOne);
    }

    public INavigationBuilder<T, TNav> HasMany<TNav>(Expression<Func<T, IEnumerable<TNav>>> navigation) where TNav : class
    {
        var navigationPropertyName = ExpressionHelper.GetNavigationPropertyName(navigation);
        _relationships.Add(new RelationshipInfo
        {
            NavigationPropertyName = navigationPropertyName,
            TargetType = typeof(TNav),
            Cardinality = RelationshipType.OneToMany
        });
        return new NavigationBuilder<T, TNav>(this, RelationshipType.OneToMany);
    }

    internal void RegisterRelationship(
        Type targetType,
        RelationshipType cardinality,
        string foreignKeyPropertyName,
        bool foreignKeyOnSource)
    {
        var existingRelationship = _relationships.FirstOrDefault(r =>
            r.TargetType == targetType && r.Cardinality == cardinality);

        if (existingRelationship != null)
        {
            existingRelationship.ForeignKeyPropertyName = foreignKeyPropertyName;
            existingRelationship.ForeignKeyOnSource = foreignKeyOnSource;

        }
    }

    public void BuildDescriptor(ModelDescriptor descriptor)
    {
        if (_key != null)
        {
            descriptor.AddKey(_key);
        }

        foreach (var relationship in _relationships)
        {
            if (relationship.ForeignKeyPropertyName == null)
            {
                throw new MappingException(
                    $"Navigation property '{relationship.NavigationPropertyName}' on type '{typeof(T).Name}' " +
                    $"to '{relationship.TargetType.Name}' has no foreign key defined. ");
            }

            descriptor.AddRelationship(new RelationshipDescriptor
            {
                NavigationPropertyName = relationship.NavigationPropertyName,
                SourceType = typeof(T),
                TargetType = relationship.TargetType,
                Cardinality = relationship.Cardinality,
                ForeignKeyPropertyNames = [relationship.ForeignKeyPropertyName],
                ForeignKeyColumnNames = [relationship.ForeignKeyPropertyName], // For now, assume column name matches property name. Phase 2 will handle [Column] overrides.
                ForeignKeyOnSource = relationship.ForeignKeyOnSource,
                HasInverse = false
            });
        }
    }

    private class RelationshipInfo
    {
        public required string NavigationPropertyName { get; set; } = null!;
        public required Type TargetType { get; set; } = null!;
        public required RelationshipType Cardinality { get; set; }
        public string? ForeignKeyPropertyName { get; set; }
        public bool ForeignKeyOnSource { get; set; }
    }
}
