namespace Qaeq.Mapping.Attributes;

/// <summary>
/// Indicates that a property represents a navigation to a related entity.
/// The property must be registered in <see cref="Qaeq.Mapping.QaeqRegistry"/> via
/// <c>HasOne</c> or <c>HasMany</c>, otherwise <see cref="Exceptions.MappingException"/>
/// is thrown during model descriptor construction.
/// </summary>
public sealed class NavigationAttribute : Attribute
{
}
