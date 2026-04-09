using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("OrderItem")]
public class OrderItem
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("OrderId")]
    public int OrderId { get; set; }

    [Column("ProductName")]
    public string ProductName { get; set; } = string.Empty;

    [Column("Quantity")]
    public int Quantity { get; set; }

    [Column("UnitPrice")]
    public decimal UnitPrice { get; set; }
}
