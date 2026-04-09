using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("ContradictoryChild")]
public class ContradictoryFkChild
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("ParentId")]
    public int ParentId { get; set; }

    [Column("OtherParentId")]
    public int OtherParentId { get; set; }

    [Navigation]
    public ContradictoryFkParent? Parent { get; set; }
}
