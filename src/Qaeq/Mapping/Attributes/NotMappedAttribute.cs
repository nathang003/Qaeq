namespace Qaeq.Mapping.Attributes;

/// <summary>
/// Indicates that a property should not be mapped to a database column
/// or treated as a navigation property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class NotMappedAttribute : Attribute
{
}
