using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates;

public class SyntaxReceiver : ISyntaxContextReceiver
{
    private CompilationContext? _compilationContext;
    public CompilationContext CompilationContext => _compilationContext ?? throw new NotSupportedException("The compilation context was not yet generated");

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        _compilationContext ??= new CompilationContext(context.SemanticModel.Compilation);

        if (context.Node is ClassDeclarationSyntax classDeclaration)
        {
            CompilationContext.AddClass(classDeclaration);
        }
    }
}