using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Qaeq.Analyzers.Test
{
    public static partial class CSharpAnalyzerVerifier<TAnalyzer>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public class Test : CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId)?.CompilationOptions;
                    if (compilationOptions is null)
                        return solution;
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

                    return solution;
                });
            }
        }
    }
}
