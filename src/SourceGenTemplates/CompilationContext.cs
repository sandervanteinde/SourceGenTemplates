using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenTemplates;

public class CompilationContext(Compilation contextSemanticModel)
{
    private readonly List<ClassDeclarationSyntax> classes = new();

    public IReadOnlyCollection<ClassDeclarationSyntax> Classes => classes;

    public void AddClass(ClassDeclarationSyntax @class)
    {
        classes.Add(@class);
    }

    public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
    {
        return contextSemanticModel.GetSemanticModel(syntaxTree);
    }
}