using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Orders")]
public class OrderToCustomerNoNav
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("CustomerId")]
    public int CustomerId { get; set; }

    [Column("Total")]
    public decimal Total { get; set; }

    [Navigation]
    public CustomerNoNav? Customer { get; set; }
}
