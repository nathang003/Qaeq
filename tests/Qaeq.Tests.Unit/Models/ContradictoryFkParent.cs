using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("ContradictoryParent")]
public class ContradictoryFkParent
{
    [Column("Id")]
    public int Id { get; set; }

    [Navigation]
    public List<ContradictoryFkChild> Children { get; set; } = [];
}
