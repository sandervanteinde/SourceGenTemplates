using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates;

public class SyntaxReceiver : ISyntaxContextReceiver
{
    public CompilationContext CompilationContext { get; } = new();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is ClassDeclarationSyntax classDeclaration)
        {
            CompilationContext.AddClass(classDeclaration);
        }
    }
}