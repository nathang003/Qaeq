using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Conflicting")]
public class ConflictingAttributeEntity
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("Value")]
    [Navigation]
    public string? Broken { get; set; }
}
