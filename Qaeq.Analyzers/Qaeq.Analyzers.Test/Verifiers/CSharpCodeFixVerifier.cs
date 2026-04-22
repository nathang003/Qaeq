using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Qaeq.Analyzers.Test;

namespace Qaeq.Tests.Analyzers.Verifiers;

public static class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
     where TAnalyzer : DiagnosticAnalyzer, new()
     where TCodeFix : CodeFixProvider, new()
{
    public static DiagnosticResult Diagnostic(string diagnosticId)
        => CSharpAnalyzerVerifier<TAnalyzer>.Diagnostic(diagnosticId);

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => CSharpAnalyzerVerifier<TAnalyzer>.Diagnostic(descriptor);

    public static async Task VerifyAnalyzerAsync(string source)
    {
        var test = new Test
        {
            TestCode = source
        };
        await test.RunAsync();
    }

    public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
    {
        var test = new Test
        {
            TestCode = source,
            FixedCode = fixedSource
        };

        test.ExpectedDiagnostics.Add(expected);
        await test.RunAsync();
    }

    public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, XUnitVerifier>
    {
    }
}
