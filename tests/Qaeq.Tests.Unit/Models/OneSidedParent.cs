using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Parents")]
public class OneSidedParent
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Navigation]
    public List<OneSidedChild> Children { get; set; } = [];
}
