using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Order")]
public class Order
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("CustomerId")]
    public int CustomerId { get; set; }

    [Column("Total")]
    public decimal Total { get; set; }

    [Column("CreatedDate")]
    public DateTime CreatedDate { get; set; }

    [Navigation]
    public Customer? Customer { get; set; }

    [Navigation]
    public List<OrderItem> Items { get; set; } = [];
}
