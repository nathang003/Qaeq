using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("CustomColumns")]
public class CustomColumnNameEntity
{
    [Column("entity_id", Order = 0, TypeName = "int")]
    public int Id { get; set; }

    [Column("display_name", Order = 1, TypeName = "nvarchar(100)")]
    public string Name { get; set; } = string.Empty;
}
