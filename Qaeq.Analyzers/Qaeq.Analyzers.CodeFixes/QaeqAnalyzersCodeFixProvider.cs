using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qaeq.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(QaeqAnalyzersCodeFixProvider)), Shared]
public class QaeqAnalyzersCodeFixProvider : CodeFixProvider
{
    private const string DiagnosticId = "QAEQ001";
    private const string AddNotMappedTitle = "Add [NotMapped]";

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create("QAEQ001");

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        var diagnostic = context.Diagnostics.FirstOrDefault(d => d.Id == DiagnosticId);
        if (diagnostic == null)
            return;

        var propertyDeclaration = root.FindToken(diagnostic.Location.SourceSpan.Start)
            .Parent?
            .AncestorsAndSelf()
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault();

        if (propertyDeclaration == null)
            return;

        // If analyzer and code fix get out of sync, avoid offering an incorrect fix.
        if (HasAnyMappingAttribute(propertyDeclaration))
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                title: AddNotMappedTitle,
                createChangedDocument: c => AddNotMappedAttributeAsync(context.Document, propertyDeclaration.SpanStart, c),
                equivalenceKey: AddNotMappedTitle),
            diagnostic);
    }

    private static async Task<Document> AddNotMappedAttributeAsync(Document document, int propertySpanStart, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false) as CompilationUnitSyntax;
        if (root == null)
            return document;

        var propertyDeclaration = root.FindToken(propertySpanStart)
            .Parent?
            .AncestorsAndSelf()
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault();

        if (propertyDeclaration == null || HasMappingAttribute(propertyDeclaration, "NotMapped"))
        {
            return document;
        }

        var notMappedAttribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("NotMapped"));
        var notMappedAttributeList = SyntaxFactory.AttributeList(
            SyntaxFactory.SingletonSeparatedList(notMappedAttribute));

        var updatedProperty = propertyDeclaration!
            .WithAttributeLists(propertyDeclaration.AttributeLists.Insert(0, notMappedAttributeList))
            .WithAdditionalAnnotations(Formatter.Annotation);

        var updatedRoot = root.ReplaceNode(propertyDeclaration, updatedProperty);

        // Ensure short attribute form compiles.
        if (!updatedRoot.Usings.Any(u => u.Name?.ToString() == "Qaeq.Mapping.Attributes"))
        {
            updatedRoot = updatedRoot.AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Qaeq.Mapping.Attributes")));
        }

        updatedRoot = updatedRoot.WithAdditionalAnnotations(Formatter.Annotation);
        return document.WithSyntaxRoot(updatedRoot);
    }

    private static bool HasAnyMappingAttribute(PropertyDeclarationSyntax propertyDeclaration) =>
        HasMappingAttribute(propertyDeclaration, "Column")
        || HasMappingAttribute(propertyDeclaration, "Navigation")
        || HasMappingAttribute(propertyDeclaration, "NotMapped");

    private static bool HasMappingAttribute(PropertyDeclarationSyntax propertyDeclaration, string attributeName)
    {
        foreach (var attribute in propertyDeclaration.AttributeLists.SelectMany(list => list.Attributes))
        {
            var name = attribute.Name.ToString();

            if (name == attributeName
                || name == attributeName + "Attribute"
                || name.EndsWith("." + attributeName)
                || name.EndsWith("." + attributeName + "Attribute"))
            {
                return true;
            }
        }

        return false;
    }
}
