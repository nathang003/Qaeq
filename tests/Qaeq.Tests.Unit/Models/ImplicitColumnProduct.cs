using Qaeq.Mapping.Attributes;

namespace Qaeq.Tests.Unit.Models;

[Table("Products", ExplicitColumns = false)]
public class ImplicitColumnProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    [NotMapped]
    public string ComputedDisplay => $"{Name} - {Price:C}";
}
