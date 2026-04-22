using Xunit;
using VerifyCS = Qaeq.Analyzers.Test.CSharpCodeFixVerifier<
    Qaeq.Analyzers.QaeqMappingAnalyzer,
    Qaeq.Analyzers.QaeqAnalyzersCodeFixProvider>;

namespace Qaeq.Analyzers.Test;

public class QAEQ001CodeFixTests
{
    [Fact]
    public async Task Adds_NotMapped_To_Virtual_Property()
    {
        var test = @"
using Qaeq.Mapping.Attributes;

[Table(""users"", ExplicitColumns = false)]
internal sealed class User
{
    public virtual string {|#0:DisplayName|} { get; set; }
}";
        var fixCode = @"
using Qaeq.Mapping.Attributes;

[Table(""users"", ExplicitColumns = false)]
internal sealed class User
{
    [NotMapped]
    public virtual string DisplayName { get; set; }
}";
        var expected = VerifyCS.Diagnostic("QAEQ001")
            .WithLocation(0)
            .WithArguments("DisplayName", "User");
        await VerifyCS.VerifyCodeFixAsync(test, expected, fixCode);
    }

    [Fact]
    public async Task Adds_Using_When_Missing()
    {
        var test = @"
[Qaeq.Mapping.Attributes.Table(""users"", ExplicitColumns = false)]
internal sealed class User
{
    public virtual string {|#0:Name|} { get; set; }

}";

        var fixedCode = @"
using Qaeq.Mapping.Attributes;

[Qaeq.Mapping.Attributes.Table(""users"", ExplicitColumns = false)]
internal sealed class User
{
    [NotMapped]
    public virtual string Name { get; set; }
}";

        var expected = VerifyCS.Diagnostic("QAEQ001")
            .WithLocation(0)
            .WithArguments("Name", "User");
        await VerifyCS.VerifyCodeFixAsync(test, expected, fixedCode);
    }
}
