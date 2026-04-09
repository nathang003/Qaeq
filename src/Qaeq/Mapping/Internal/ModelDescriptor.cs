using Qaeq.Exceptions;
using Qaeq.Mapping.Attributes;
using Qaeq.Mapping.Descriptors;
using System.Reflection;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Internal implementation of <see cref="IModelDescriptor"/>
/// </summary>
internal class ModelDescriptor : IModelDescriptor, IModelDescriptorInternal
{
    private readonly List<ColumnDescriptor> _columns = [];
    private readonly List<KeyDescriptor> _keys = [];
    private readonly List<RelationshipDescriptor> _relationships = [];

    public required Type ClrType { get; init; }
    public required string TableName { get; init; }
    public required string Schema { get; init; }
    public bool ExplicitColumns { get; init; } = true;

    public IReadOnlyList<ColumnDescriptor> Columns => _columns;
    public IReadOnlyList<KeyDescriptor> Keys => _keys;
    public IReadOnlyList<RelationshipDescriptor> Relationships => _relationships;

    /// <summary>
    /// Mutable access to relationships for cross-entity validation
    /// </summary>
    List<RelationshipDescriptor> IModelDescriptorInternal.MutableRelationships => _relationships;

    public void AddColumn(ColumnDescriptor column)
    {
        _columns.Add(column);
    }

    public void AddKey(KeyDescriptor key)
    {
        _keys.Add(key);
    }

    public void AddRelationship(RelationshipDescriptor relationship)
    {
        _relationships.Add(relationship);
    }

    /// <summary>
    /// Builds the complete model descriptor for a CLR type by merging attribute data and registry data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ModelDescriptor Build<T>(IEntityBuilderInternal builder) where T : class
    {
        return BuildByType(typeof(T), builder);
    }

    /// <summary>
    /// Non-generic build method for use when the type is known only at runtime.
    /// </summary>
    /// <param name="clrType"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="MappingException"></exception>
    public static ModelDescriptor BuildByType(Type clrType, IEntityBuilderInternal builder)
    {
        var tableAttribute = clrType.GetCustomAttribute<TableAttribute>();

        if (tableAttribute == null)
        {
            throw new MappingException(
                $"Type '{clrType.Name}' is not decorated with [Table] attribute.");
        }

        var descriptor = new ModelDescriptor
        {
            ClrType = clrType,
            TableName = tableAttribute.Name,
            Schema = tableAttribute.Schema,
            ExplicitColumns = tableAttribute.ExplicitColumns
        };

        // Build columns based on ExplicitColumns and property attributes
        var properties = clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var hasColumn = property.GetCustomAttribute<ColumnAttribute>() != null;
            var hasNavigation = property.GetCustomAttribute<NavigationAttribute>() != null;
            var hasNotMapped = property.GetCustomAttribute<NotMappedAttribute>() != null;
            var isVirtual = property.GetGetMethod()?.IsVirtual == true
                && property.GetGetMethod()?.IsFinal == false;

            // Check for conflicting attributes
            var attributeCount = (hasColumn ? 1 : 0) + (hasNavigation ? 1 : 0) + (hasNotMapped ? 1 : 0);
            if (attributeCount > 1)
            {
                throw new MappingException(
                    $"Property '{property.Name}' on type '{clrType.Name}' has multiple mapping attributes. " +
                    $"A property cannot have more than one of [Column], [Navigation], or [NotMapped].");
            }

            // Determine if this property should be mapped
            bool shouldMapAsColumn = false;

            if (descriptor.ExplicitColumns)
            {
                // Only explicit [Column] attributes are mapped
                if (hasColumn)
                {
                    shouldMapAsColumn = true;
                }

                // hasNavigation -> handled after builder populates relationships, but we validate that it's registered
                // hasNotMapped or no attributes -> ignored
            }
            else
            {
                // Implicit mapping: all public non-virtual properties are columns
                if (hasNotMapped)
                {
                    // Ignored
                }
                else if (hasNavigation)
                {
                    // Navigation -- handled after builder populates relationships, but we validate that it's registered
                }
                else if (hasColumn)
                {
                    shouldMapAsColumn = true;
                }
                else if (!isVirtual)
                {
                    shouldMapAsColumn = true;
                }
                // isVirtual without attributes -> ignored (analyzer will warn)
            }

            if (shouldMapAsColumn)
            {
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                var columnName = columnAttribute?.Name ?? property.Name;
                var sqlType = columnAttribute?.TypeName;
                var order = columnAttribute?.Order ?? -1;
                var isNullable = IsNullableValueType(property.PropertyType) ||
                    (property.PropertyType.IsClass && property.PropertyType != typeof(string));

                descriptor.AddColumn(new ColumnDescriptor
                {
                    PropertyName = property.Name,
                    ColumnName = columnName,
                    ClrType = property.PropertyType,
                    SqlTypeName = sqlType,
                    Order = order,
                    IsNullable = isNullable
                });
            }
        }

        // Add keys and relationships from the registry
        builder.BuildDescriptor(descriptor);

        // Validate if any navigation property exists on the type but wasn't registered
        foreach (var property in properties)
        {
            var hasNavigation = property.GetCustomAttribute<NavigationAttribute>() != null;
            if (hasNavigation)
            {
                var hasRelationship = descriptor._relationships.Any(r => r.NavigationPropertyName == property.Name);
                if (!hasRelationship)
                {
                    throw new MappingException(
                        $"Property '{property.Name}' on type '{clrType.Name}' has a [Navigation] attribute " +
                        $"but is not registered via HasOne or HasMany in QaeqRegistry.");
                }
            }
        }

        return descriptor;
    }

    private static bool IsNullableValueType(Type type)
    {
        return type.IsValueType && Nullable.GetUnderlyingType(type) != null;
    }

}
