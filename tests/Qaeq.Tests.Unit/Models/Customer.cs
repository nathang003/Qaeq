using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Customer")]
public class Customer
{
    [Column("Id")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Email")]
    public string Email { get; set; } = string.Empty;

    [Navigation]
    public List<Order> Orders { get; set; } = [];
}
