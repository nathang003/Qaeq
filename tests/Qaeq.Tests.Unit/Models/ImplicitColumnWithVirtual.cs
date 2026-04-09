using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("VirtualTest", ExplicitColumns = false)]
public class ImplicitColumnWithVirtual
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual string? SomeVirtualProp { get; set; }
}
