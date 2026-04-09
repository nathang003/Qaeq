using Qaeq.Mapping.Descriptors;

namespace Qaeq.Mapping.Internal;

/// <summary>
/// Internal interface exposing mutable members of ModelDescriptor.
/// Used by ModelDescriptorCache for relationship validation and patching after all descriptors are built.
/// </summary>
internal interface IModelDescriptorInternal : IModelDescriptor
{
    /// <summary>
    /// Provides mutable access to relationships for validation patching.
    /// </summary>
    List<RelationshipDescriptor> MutableRelationships { get; }
}
