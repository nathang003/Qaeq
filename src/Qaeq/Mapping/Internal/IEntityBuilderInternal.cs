namespace Qaeq.Mapping.Internal;

/// <summary>
/// Non-generic internal interface for storing entity builders in a type-erased collection.
/// </summary>
internal interface IEntityBuilderInternal
{
    /// <summary>
    /// The entity CLR type this builder configures.
    /// </summary>
    Type EntityType { get; }

    /// <summary>
    /// Populates the given model descriptor with keys and relationships from this builder.
    /// </summary>
    /// <param name="descriptor"></param>
    void BuildDescriptor(ModelDescriptor descriptor);
}
