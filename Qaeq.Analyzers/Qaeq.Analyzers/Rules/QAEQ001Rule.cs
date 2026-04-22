using Microsoft.CodeAnalysis;

namespace Qaeq.Analyzers.Rules;

internal static class QAEQ001Rule
{
    public const string DiagnosticId = "QAEQ001";

    private static readonly LocalizableString _title =
        new LocalizableResourceString(nameof(Resources.QAEQ001_Title), Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString _messageFormat =
        new LocalizableResourceString(nameof(Resources.QAEQ001_MessageFormat), Resources.ResourceManager, typeof(Resources));

    private static readonly LocalizableString _description =
        new LocalizableResourceString(nameof(Resources.QAEQ001_Description), Resources.ResourceManager, typeof(Resources));

    private const string Category = "Usage";

    public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        _title,
        _messageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: _description,
        helpLinkUri: "https://github.com/nathang003/Qaeq/docs/rules/QAEQ001");
}
