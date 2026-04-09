using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Children")]
public class OneSidedChild
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("ParentId")]
    public int ParentId { get; set; }

    [Column("Value")]
    public string Value { get; set; } = string.Empty;

    // No [Navigation] back to parent — deliberately one-sided
}
