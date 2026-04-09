using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Orphaned")]
public class OrphanedNavigationEntity
{
    [Column("Id")]
    public int Id { get; set; }

    [Navigation]
    public Customer? Customer { get; set; }
}
