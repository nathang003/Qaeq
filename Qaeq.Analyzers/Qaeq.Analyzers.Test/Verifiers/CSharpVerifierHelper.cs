using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Qaeq.Analyzers.Test
{
    internal static class CSharpVerifierHelper
    {
        /// <summary>
        /// By default, the compiler reports diagnostics for nullable reference types at
        /// <see cref="DiagnosticSeverity.Warning"/>, and the analyzer test framework defaults to only validating
        /// diagnostics at <see cref="DiagnosticSeverity.Error"/>. This map contains all compiler diagnostic IDs
        /// related to nullability mapped to <see cref="ReportDiagnostic.Error"/>, which is then used to enable all
        /// of these warnings for default validation during analyzer and code fix tests.
        /// </summary>
        internal static ImmutableDictionary<string, ReportDiagnostic> NullableWarnings { get; } = GetNullableWarnings();

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarnings()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, ReportDiagnostic>();

            // Nullable reference type warning codes (CS8600–CS8699 range).
            // Covers the core nullable warnings introduced in C# 8.0 and expanded through C# 11+.
            // Corresponds to the set enabled by /warnaserror:nullable in the Roslyn compiler.
            for (int i = 8600; i <= 8699; i++)
            {
                builder[$"CS{i}"] = ReportDiagnostic.Error;
            }

            // Additional nullable-related warnings outside the main CS86xx range.
            // CS8597: Thrown value may be null.
            // CS8762–CS8777: Nullability postcondition/precondition attribute violations.
            // CS8794, CS8819: Pattern matching nullability warnings.
            // CS8824–CS8825: Nullability of parameter/return type mismatch.
            foreach (var id in new[]
            {
                "CS8597", "CS8762", "CS8763", "CS8764", "CS8765", "CS8766",
                "CS8767", "CS8768", "CS8769", "CS8770", "CS8774", "CS8775",
                "CS8776", "CS8777", "CS8794", "CS8819", "CS8824", "CS8825",
            })
            {
                builder[id] = ReportDiagnostic.Error;
            }

            return builder.ToImmutable();
        }
    }
}
