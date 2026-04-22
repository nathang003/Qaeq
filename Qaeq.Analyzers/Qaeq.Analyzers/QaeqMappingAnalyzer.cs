using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Qaeq.Analyzers.Rules;
using System.Collections.Immutable;

namespace Qaeq.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class QaeqMappingAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(QAEQ001Rule.Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeClassSymbol, SymbolKind.NamedType);
    }

    private void AnalyzeClassSymbol(SymbolAnalysisContext context)
    {
        var classSymbol = (INamedTypeSymbol)context.Symbol;

        // Only analyze classes
        if (classSymbol.TypeKind != TypeKind.Class)
            return;

        // Check if class has [Table] attribute
        var tableAttribute = classSymbol.GetAttributes()
            .FirstOrDefault(attribute =>
               attribute.AttributeClass?.Name == "TableAttribute" &&
               attribute.AttributeClass.ContainingNamespace.Name == "Qaeq" &&
               attribute.AttributeClass.ContainingNamespace.ContainingNamespace?.Name == "Mapping" &&
               attribute.AttributeClass.ContainingNamespace.ContainingNamespace?.ContainingNamespace?.Name == "Attributes");

        if (tableAttribute == null)
            return;

        // Check if ExplicitColumns is false
        bool explicitColumns = true; // Default to true if not specified
        var explicitColumnsArgument = tableAttribute.NamedArguments
            .FirstOrDefault(arg => arg.Key == "ExplicitColumns");

        if (explicitColumnsArgument.Value.Value is bool explicitValue)
        {
            explicitColumns = explicitValue;
        }

        if (explicitColumns)
            return; // Only check when ExplicitColumns is false

        // Get all public properties
        var properties = classSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(prop => prop.DeclaredAccessibility == Accessibility.Public);

        foreach (var property in properties)
        {
            // Check if property is virtual
            if (!IsVirtualProperty(property))
                continue;

            // Check if property has mapping attributes
            bool hasColumnAttribute = property.GetAttributes()
                .Any(attribute => attribute.AttributeClass?.Name == "ColumnAttribute");

            bool hasNavigationAttribute = property.GetAttributes()
                .Any(attribute => attribute.AttributeClass?.Name == "NavigationAttribute");

            bool hasNotMappedAttribute = property.GetAttributes()
                .Any(attribute => attribute.AttributeClass?.Name == "NotMappedAttribute");

            // If virtual property and no mapping attributes, report diagnostic
            if (!hasColumnAttribute && !hasNavigationAttribute && !hasNotMappedAttribute)
            {
                var diagnostic = Diagnostic.Create(
                    QAEQ001Rule.Rule,
                    property.Locations[0],
                    property.Name,
                    classSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private bool IsVirtualProperty(IPropertySymbol property)
    {
        // A property is virtual if its getter or setter is virtual and not final
        var getter = property.GetMethod;
        var setter = property.SetMethod;

        return (getter?.IsVirtual == true && getter.IsSealed == false) ||
            (setter?.IsVirtual == true && setter.IsSealed == false);
    }
}
