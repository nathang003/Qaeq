using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("TenantOrders")]
public class CompositeKeyEntity
{
    [Column("TenantId")]
    public int TenantId { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }

    [Column("Description")]
    public string Description { get; set; } = string.Empty;
}
