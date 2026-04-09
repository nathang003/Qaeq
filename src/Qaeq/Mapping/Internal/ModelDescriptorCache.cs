using Qaeq.Exceptions;
using System.Collections.Concurrent;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Thread-safe cache for model descriptors.
/// </summary>
internal static class ModelDescriptorCache
{
    private static ConcurrentDictionary<Type, IModelDescriptorInternal> _cache = new();
    private static bool _validated;

    /// <summary>
    /// Clears all cached descriptors. Intended for testing only.
    /// </summary>
    internal static void Clear()
    {
        _cache.Clear();
        _validated = false;
    }

    public static IModelDescriptor GetDescriptor<T>(MetadataRegistry registry) where T : class
    {
        var clrType = typeof(T);

        // Build the requested descriptor
        var descriptor = GetOrBuild<T>(registry);

        // on first access, build all registered types and validate cross-entity relationships
        if (!_validated)
        {
            BuildAllAndValidate(registry);
        }

        return descriptor;
    }

    private static IModelDescriptor GetOrBuild<T>(MetadataRegistry registry) where T : class
    {
        var clrType = typeof(T);

        return _cache.GetOrAdd(clrType, _ =>
        {
            var builder = registry.TryGetBuilder<T>();

            if (builder == null)
            {
                throw new MappingException(
                    $"Type '{clrType.Name}' has not been registered in QaeqRegistry. " +
                    $"Call QaeqRegistry.Configure and register the entity.");
            }

            return ModelDescriptor.Build<T>(builder);
        });
    }

    private static void BuildAllAndValidate(MetadataRegistry registry)
    {
        // Build all registered descriptors
        var allTypes = registry.GetRegisteredTypes();
        foreach (var type in allTypes)
        {
            _cache.GetOrAdd(type, t =>
            {
                var builder = registry.TryGetBuilderByType(t);
                if (builder == null)
                {
                    throw new MappingException(
                        $"Type '{t.Name}' has not been registered in QaeqRegistry.");
                }

                return ModelDescriptor.BuildByType(t, builder);
            });
        }

        // Now validate cross-entity relationships
        ValidateRelationships();

        _validated = true;
    }

    private static void ValidateRelationships()
    {
        var allDescriptors = _cache.Values.ToList();

        foreach (var descriptor in allDescriptors)
        {
            for (var i = 0; i < descriptor.MutableRelationships.Count; i++)
            {
                var relationship = descriptor.MutableRelationships[i];

                // Find the target descriptor
                var targetDescriptor = allDescriptors.FirstOrDefault(d => d.ClrType == relationship.TargetType);
                if (targetDescriptor == null)
                    continue;

                // Look for an inverse relationship on the target descriptor
                for (var j = 0; j < targetDescriptor.MutableRelationships.Count; j++)
                {
                    var candidate = targetDescriptor.MutableRelationships[j];

                    // Check if this candidate is the inverse of the original relationship
                    if (candidate.TargetType != relationship.SourceType)
                        continue;

                    var isValidInverse =
                        (relationship.Cardinality == RelationshipType.ManyToOne && candidate.Cardinality == RelationshipType.OneToMany) ||
                        (relationship.Cardinality == RelationshipType.OneToMany && candidate.Cardinality == RelationshipType.ManyToOne) ||
                        (relationship.Cardinality == RelationshipType.OneToOne && candidate.Cardinality == RelationshipType.OneToOne);

                    if (!isValidInverse)
                        continue;

                    // Validate consistency -- FK property counts must match
                    if (relationship.ForeignKeyPropertyNames.Count != candidate.ForeignKeyPropertyNames.Count)
                    {
                        throw new MappingException(
                            $"Relationship between '{relationship.SourceType.Name}.{relationship.NavigationPropertyName}' " +
                            $"and '{candidate.SourceType.Name}.{candidate.NavigationPropertyName}' has contradictory foreign keys: " +
                            $"one side uses {relationship.ForeignKeyPropertyNames.Count} key(s), " +
                            $"the other side uses {candidate.ForeignKeyPropertyNames.Count} key(s).");
                    }

                    // Validate FK property names match
                    var relFkNames = relationship.ForeignKeyOnSource
                        ? relationship.ForeignKeyPropertyNames
                        : relationship.ForeignKeyPropertyNames;
                    var candidateFkNames = candidate.ForeignKeyOnSource
                        ? candidate.ForeignKeyPropertyNames
                        : candidate.ForeignKeyPropertyNames;

                    if (!relFkNames.SequenceEqual(candidateFkNames))
                    {
                        throw new MappingException(
                            $"Relationship between '{relationship.SourceType.Name}.{relationship.NavigationPropertyName}' " +
                            $"and '{candidate.SourceType.Name}.{candidate.NavigationPropertyName}' has contradictory foreign key properties: " +
                            $"one side references [{string.Join(", ", relFkNames)}], " +
                            $"the other references [{string.Join(", ", candidateFkNames)}].");
                    }

                    // Mark both sides as having an inverse
                    descriptor.MutableRelationships[i] = relationship with { HasInverse = true };
                    targetDescriptor.MutableRelationships[j] = candidate with { HasInverse = true };

                }
            }
        }
    }
}
